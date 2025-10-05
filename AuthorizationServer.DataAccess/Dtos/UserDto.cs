using System.Text.Json.Serialization;

namespace AuthorizationServer.DataAccess.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    [JsonIgnore]
    public string Password { get; set; }
    
    public DateTime CreatedAt { get; set; }
}