namespace AuthorizationServer.DataAccess.Dtos.Application;

public class CreateApplicationDto
{
    public string Name { get; set; }
    public List<string> RedirectUrls { get; set; }
    public string? Description { get; set; } 
}