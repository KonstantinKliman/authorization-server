namespace AuthorizationServer.DataAccess.Entities;

public class UserApplication
{
    public Guid UserId { get; set; }
    
    public User User { get; set; }
    
    public Guid ApplicationId { get; set; }
    
    public Application Application { get; set; }
    
    public List<string> Roles { get; set; }
}