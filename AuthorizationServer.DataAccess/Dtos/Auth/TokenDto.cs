namespace AuthorizationServer.DataAccess.Dtos.Auth;

public class TokenDto
{
    public string? AccessToken { get; set; }
    public string? RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
}