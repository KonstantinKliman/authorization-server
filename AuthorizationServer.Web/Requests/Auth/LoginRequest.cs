namespace AuthorizationServer.Web.Requests.Auth;

public class LoginRequest
{
    public string Login { get; set; }
    
    public string Password { get; set; }
}