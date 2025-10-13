using System.Security.Claims;

namespace AuthorizationServer.BusinessLogic.Interfaces.Services;

public interface IJwtService
{
    string GenerateAccessToken(Guid userId, string username, List<string> roles, Guid applicationId);
    string GenerateRefreshToken();
    string GenerateAuthorizationCode();
    ClaimsPrincipal? ValidateToken(string token);
}