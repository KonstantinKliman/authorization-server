namespace AuthorizationServer.Web.Dto;

public class CreateClientDto
{
    public string ClientId { get; set; } = default!;
    // "public" для SPA/мобилок, "confidential" для серверных приложений
    public string ClientType { get; set; } = "public";
    public string? DisplayName { get; set; }

    // Только для confidential-клиентов
    public string? ClientSecret { get; set; }

    public bool AllowAuthorizationCode { get; set; } = true;
    public bool AllowClientCredentials { get; set; } = false;
    public bool AllowRefreshToken { get; set; } = true;
    public bool RequirePkce { get; set; } = true; // обязательно для SPA/мобилок

    public List<string>? RedirectUris { get; set; }
    public List<string>? PostLogoutRedirectUris { get; set; }
    public List<string>? AllowedScopes { get; set; } // например: ["api", "feedback.read"]
}