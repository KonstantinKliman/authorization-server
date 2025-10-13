using AuthorizationServer.BusinessLogic.Configuration;
using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.DataAccess.Context;
using AuthorizationServer.DataAccess.Dtos.Auth;
using AuthorizationServer.DataAccess.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthorizationServer.BusinessLogic.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly IPasswordService _passwordService;
    private readonly IJwtService _jwtService;
    private readonly JwtSettings _jwtSettings;
    
    public AuthService(
        AppDbContext context,
        IPasswordService passwordService,
        IJwtService jwtService,
        IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }
    
    public async Task<Result<AuthorizationCodeDto>> LoginAsync(string username, string password, string clientId, string redirectUri)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Name == username);
        
        if (user == null)
            return Result.Fail(new Error("Неверный логин"));
        
        if (!_passwordService.Verify(password, user.PasswordHash))
            return Result.Fail(new Error("Неверный пароль"));
        
        var application = await _context.Applications
            .FirstOrDefaultAsync(a => a.ClientId == clientId);

        if (application == null)
            return Result.Fail(new Error("Приложение не найдено"));
        
        if (!application.RedirectUrls.Contains(redirectUri))
            return Result.Fail(new Error("Неверный redirect URI"));
        
        var userApp = await _context.UserApplications
            .FirstOrDefaultAsync(ua => ua.UserId == user.Id && ua.ApplicationId == application.Id);

        if (userApp == null)
            return Result.Fail(new Error("У вас нет доступа к этому приложению"));
        
        // authorization code
        var code = _jwtService.GenerateAuthorizationCode();
        var authCode = new AuthorizationCode
        {
            Code = code,
            UserId = user.Id,
            ApplicationId = application.Id,
            RedirectUri = redirectUri,
            ExpiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AuthorizationCodeExpirationMinutes),
            CreatedAt = DateTime.UtcNow
        };

        _context.AuthorizationCodes.Add(authCode);
        await _context.SaveChangesAsync();

        return Result.Ok(new AuthorizationCodeDto() { Code = code, RedirectUri = redirectUri });
    }

    public async Task<Result<TokenDto>> ExchangeCodeForTokensAsync(string code, string clientId, string clientSecret)
    {
        // 1. Найти authorization code
        var authCode = await _context.AuthorizationCodes
            .Include(ac => ac.User)
            .Include(ac => ac.Application)
            .FirstOrDefaultAsync(ac => ac.Code == code);

        if (authCode == null || authCode.IsUsed)
            return Result.Fail(new Error("Неверный или использованный код"));

        // 2. Проверить срок действия
        if (authCode.ExpiresAt < DateTime.UtcNow)
            return Result.Fail(new Error("Код истек"));

        // 3. Проверить clientId и clientSecret
        if (authCode.Application.ClientId != clientId || 
            !_passwordService.Verify(clientSecret, authCode.Application.ClientSecret))
            return Result.Fail(new Error("Неверные учетные данные приложения"));

        // 4. Пометить код как использованный
        authCode.IsUsed = true;

        // 5. Получить роли пользователя
        var userApp = await _context.UserApplications
            .FirstOrDefaultAsync(ua => ua.UserId == authCode.UserId && 
                                      ua.ApplicationId == authCode.ApplicationId);

        var roles = userApp?.Roles ?? new List<string>();

        // 6. Сгенерировать токены
        var accessToken = _jwtService.GenerateAccessToken(
            authCode.User.Id,
            authCode.User.Name,
            roles,
            authCode.Application.Id
        );

        var refreshTokenValue = _jwtService.GenerateRefreshToken();
        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = authCode.User.Id,
            ApplicationId = authCode.Application.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            CreatedAt = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return Result.Ok(new TokenDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            ExpiresIn = _jwtSettings.AccessTokenExpirationMinutes * 60
        });
    }

    public async Task<Result<TokenDto>> RefreshTokenAsync(string refreshToken)
    {
        var storedToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .Include(rt => rt.Application)
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedToken == null)
            return Result.Fail(new Error("Неверный refresh token"));
        
        if (storedToken.IsRevoked)
            return Result.Fail(new Error("Refresh token был отозван"));
        
        if (storedToken.ExpiresAt < DateTime.UtcNow)
            return Result.Fail(new Error("Refresh token истек"));
        
        var userApp = await _context.UserApplications
            .FirstOrDefaultAsync(ua => ua.UserId == storedToken.UserId && 
                                       ua.ApplicationId == storedToken.ApplicationId);

        var roles = userApp?.Roles ?? new List<string>();
        
        var accessToken = _jwtService.GenerateAccessToken(
            storedToken.User.Id,
            storedToken.User.Name,
            roles,
            storedToken.Application.Id
        );

        return Result.Ok(new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = _jwtSettings.AccessTokenExpirationMinutes * 60
        });
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

        if (storedToken != null && !storedToken.IsRevoked)
        {
            storedToken.IsRevoked = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Result<UserInfoDto>> GetUserInfoAsync(string accessToken)
    {
        var principal = _jwtService.ValidateToken(accessToken);
    
        if (principal == null)
            return Result.Fail(new Error("Неверный или истекший токен"));
        
        var userIdClaim = principal.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            return Result.Fail(new Error("Токен не содержит информацию о пользователе"));
        
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return Result.Fail(new Error("Пользователь не найден"));
        
        var username = principal.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;
        var applicationIdClaim = principal.FindFirst("application_id")?.Value;
        var roles = principal.FindAll(System.Security.Claims.ClaimTypes.Role)
            .Select(c => c.Value)
            .ToList();

        Guid.TryParse(applicationIdClaim, out var applicationId);

        return Result.Ok(new UserInfoDto
        {
            UserId = userId,
            Username = username ?? user.Name,
            Roles = roles,
            ApplicationId = applicationId
        });
    }
}