namespace AuthorizationServer.Web.Requests.Auth;

public class LogoutRequest
{
    public string RefreshToken { get; set; }
}