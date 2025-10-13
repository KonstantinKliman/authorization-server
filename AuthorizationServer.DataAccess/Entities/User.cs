namespace AuthorizationServer.DataAccess.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; }
    
    public string PasswordHash { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ICollection<Application> Applications { get; set; }
    
    public ICollection<UserApplication> UserApplications { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    
    public ICollection<AuthorizationCode> AuthorizationCodes { get; set; }
}