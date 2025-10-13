using AuthorizationServer.BusinessLogic.Configuration;
using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.DataAccess.Context;
using AuthorizationServer.DataAccess.Dtos.Auth;
using AuthorizationServer.DataAccess.Entities;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace AuthorizationServer.BusinessLogic.Services;

public class AdminAuthService : IAdminAuthService
{
    private readonly AppDbContext _context;

    private readonly IPasswordService _passwordService;

    private readonly IJwtService _jwtService;
    
    private readonly JwtSettings _jwtSettings;

    public AdminAuthService(AppDbContext context, IPasswordService passwordService, IJwtService jwtService, IOptions<JwtSettings> jwtSettings)
    {
        _context = context;
        _passwordService = passwordService;
        _jwtService = jwtService;
        _jwtSettings = jwtSettings.Value;
    }
    
    public async Task<Result<TokenDto>> Login(string login, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Name == login);
    
        if (user == null)
            return Result.Fail(new Error("Неверный логин или пароль"));
    
        if (!_passwordService.Verify(password, user.PasswordHash))
            return Result.Fail(new Error("Неверный логин или пароль"));
        
        var adminApp = await _context.Applications
            .FirstOrDefaultAsync(a => a.ClientId == "admin-panel");
    
        if (adminApp == null)
            return Result.Fail(new Error("Admin Panel не настроен в системе"));
        
        var userAccess = await _context.UserApplications
            .FirstOrDefaultAsync(ua => ua.UserId == user.Id && 
                                       ua.ApplicationId == adminApp.Id);
    
        if (userAccess == null)
            return Result.Fail(new Error("У вас нет доступа к панели администратора"));
        
        var accessToken = _jwtService.GenerateAccessToken(
            user.Id,
            user.Name,
            userAccess.Roles,  // роли из UserApplications
            adminApp.Id
        );
    
        var refreshTokenValue = _jwtService.GenerateRefreshToken();
        var refreshToken = new RefreshToken
        {
            Token = refreshTokenValue,
            UserId = user.Id,
            ApplicationId = adminApp.Id,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            CreatedAt = DateTime.UtcNow
        };
    
        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();
        
        return Result.Ok(new TokenDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshTokenValue,
            ExpiresIn = _jwtSettings.AccessTokenExpirationMinutes * 60
        });
    }
}