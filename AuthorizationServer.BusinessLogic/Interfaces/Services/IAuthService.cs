using AuthorizationServer.DataAccess.Dtos.Auth;
using FluentResults;

namespace AuthorizationServer.BusinessLogic.Interfaces.Services;

public interface IAuthService
{
    Task<Result<AuthorizationCodeDto>> LoginAsync(string username, string password, string clientId, string redirectUri);  // Guid -> string
    Task<Result<TokenDto>> ExchangeCodeForTokensAsync(string code, string clientId, string clientSecret);  // Guid -> string
    Task<Result<TokenDto>> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(string refreshToken);
    Task<Result<UserInfoDto>> GetUserInfoAsync(string accessToken);
}