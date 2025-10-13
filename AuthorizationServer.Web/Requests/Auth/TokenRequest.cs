namespace AuthorizationServer.Web.Requests.Auth;

public class TokenRequest
{
    public string GrantType { get; set; } // "authorization_code" или "refresh_token"
    public string? Code { get; set; } // для authorization_code
    public string ClientId { get; set; }
    public string? ClientSecret { get; set; }
    public string? RefreshToken { get; set; } // для refresh_token
}