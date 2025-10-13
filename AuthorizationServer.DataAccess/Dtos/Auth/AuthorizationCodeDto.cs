namespace AuthorizationServer.DataAccess.Dtos.Auth;

public class AuthorizationCodeDto
{
    public string? Code { get; set; }
    public string? RedirectUri { get; set; }
}