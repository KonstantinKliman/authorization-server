namespace AuthorizationServer.DataAccess.Dtos.Auth;

public class UserInfoDto
{
    public Guid UserId { get; set; }
    public string? Username { get; set; }
    public List<string>? Roles { get; set; }
    public Guid ApplicationId { get; set; }
}