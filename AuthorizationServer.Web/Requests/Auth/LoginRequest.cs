namespace AuthorizationServer.Web.Requests.Auth;

public class LoginRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
    public string RedirectUri { get; set; }
}