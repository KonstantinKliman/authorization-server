namespace AuthorizationServer.DataAccess.Entities;

public class AuthorizationCode
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public Guid UserId { get; set; }
    public User User { get; set; }
    public Guid ApplicationId { get; set; }
    public Application Application { get; set; }
    public string RedirectUri { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsUsed { get; set; } = false;
}