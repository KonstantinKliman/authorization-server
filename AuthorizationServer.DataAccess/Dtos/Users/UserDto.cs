using System.Text.Json.Serialization;

namespace AuthorizationServer.DataAccess.Dtos.Users;

public class UserDto
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime CreatedAt { get; set; }
}