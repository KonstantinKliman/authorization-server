namespace AuthorizationServer.DataAccess.Dtos.Users;

public class UpdateUserDto
{
    public Guid Id { get; set; }
    
    public string? Name { get; set; }
    
    public string? Password { get; set; }
}