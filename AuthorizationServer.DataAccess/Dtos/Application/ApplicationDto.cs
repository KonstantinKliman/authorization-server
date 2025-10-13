using System.Text.Json.Serialization;

namespace AuthorizationServer.DataAccess.Dtos.Application;

public class ApplicationDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string ClientId { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ClientSecret { get; set; }
    
    public List<string> RedirectUrls { get; set; }
    
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
}