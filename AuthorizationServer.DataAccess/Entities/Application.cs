namespace AuthorizationServer.DataAccess.Entities;

public class Application
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string ClientId { get; set; }
    
    public string ClientSecret { get; set; }
    
    public List<string> RedirectUrls { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public ICollection<User> Users { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; set; }
    
    public ICollection<UserApplication> UserApplications { get; set; }
    
    public ICollection<AuthorizationCode> AuthorizationCodes { get; set; }
}