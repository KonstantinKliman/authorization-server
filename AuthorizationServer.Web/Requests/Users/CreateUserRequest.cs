namespace AuthorizationServer.Web.Requests.Users;

public class CreateUserRequest
{
    public string Name { get; set; }
    
    public string Password { get; set; }
}