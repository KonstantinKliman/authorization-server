namespace AuthorizationServer.Web.Requests.Users;

public class UpdateUserRequest
{
    public string? Name { get; set; }
    
    public string? Password { get; set; }
}