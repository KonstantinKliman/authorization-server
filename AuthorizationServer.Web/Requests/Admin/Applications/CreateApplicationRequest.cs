namespace AuthorizationServer.Web.Requests.Admin.Applications;

public class CreateApplicationRequest
{
    public string Name { get; set; }
    public List<string> RedirectUrls { get; set; }
    public string? Description { get; set; } 
}