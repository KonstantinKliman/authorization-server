using AuthorizationServer.Web.Dto;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace AuthorizationServer.Web.Controllers;

[ApiController]
[Route("api/idp/clients")]
public class ClientController : ControllerBase
{
    private readonly IOpenIddictApplicationManager _apps;

    public ClientController(IOpenIddictApplicationManager apps) => _apps = apps;
    
    public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
    {
        // Базовая валидация
        if (string.IsNullOrWhiteSpace(dto.ClientId))
            return BadRequest("ClientId is required.");

        var type = dto.ClientType?.Equals("confidential", StringComparison.OrdinalIgnoreCase) == true
            ? OpenIddictConstants.ClientTypes.Confidential
            : OpenIddictConstants.ClientTypes.Public;

        if (type == OpenIddictConstants.ClientTypes.Confidential && string.IsNullOrWhiteSpace(dto.ClientSecret))
            return BadRequest("ClientSecret is required for confidential clients.");

        if (await _apps.FindByClientIdAsync(dto.ClientId) is not null)
            return Conflict("ClientId already exists.");

        var d = new OpenIddictApplicationDescriptor
        {
            ClientId = dto.ClientId,
            ClientType = type,
            DisplayName = dto.DisplayName,
            // Чаще всего — явный consent
            ConsentType = OpenIddictConstants.ConsentTypes.Explicit
        };

        if (type == OpenIddictConstants.ClientTypes.Confidential)
            d.ClientSecret = dto.ClientSecret; // менеджер сам захэширует

        // URIs
        foreach (var uri in dto.RedirectUris ?? Enumerable.Empty<string>())
            d.RedirectUris.Add(new Uri(uri));
        foreach (var uri in dto.PostLogoutRedirectUris ?? Enumerable.Empty<string>())
            d.PostLogoutRedirectUris.Add(new Uri(uri));

        // Permissions: endpoints/grant types/response types/scopes
        var perms = new HashSet<string>();

        if (dto.AllowAuthorizationCode)
        {
            perms.Add(OpenIddictConstants.Permissions.Endpoints.Authorization);
            perms.Add(OpenIddictConstants.Permissions.Endpoints.Token);
            perms.Add(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode);
            perms.Add(OpenIddictConstants.Permissions.ResponseTypes.Code);
        }

        if (dto.AllowClientCredentials)
        {
            perms.Add(OpenIddictConstants.Permissions.Endpoints.Token);
            perms.Add(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials);
        }

        if (dto.AllowRefreshToken)
        {
            perms.Add(OpenIddictConstants.Permissions.GrantTypes.RefreshToken);
            perms.Add(OpenIddictConstants.Permissions.Endpoints.Token);
        }

        foreach (var s in dto.AllowedScopes ?? Enumerable.Empty<string>())
            perms.Add(OpenIddictConstants.Permissions.Prefixes.Scope + s);

        d.Permissions.UnionWith(perms);

        if (dto.RequirePkce)
            d.Requirements.Add(OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange);

        await _apps.CreateAsync(d);

        // Возвращаем краткую карточку без секрета
        return CreatedAtAction(nameof(GetByClientId), new { clientId = dto.ClientId },
            new { dto.ClientId, dto.DisplayName, dto.ClientType });
    }
    
    [HttpGet("{clientId}")]
    public async Task<IActionResult> GetByClientId(string clientId)
    {
        var app = await _apps.FindByClientIdAsync(clientId);
        if (app is null) return NotFound();

        var desc = new OpenIddictApplicationDescriptor();
        await _apps.PopulateAsync(app, desc);

        var scopes = desc.Permissions
            .Where(p => p.StartsWith(OpenIddictConstants.Permissions.Prefixes.Scope, StringComparison.Ordinal))
            .Select(p => p[OpenIddictConstants.Permissions.Prefixes.Scope.Length..])
            .ToArray();

        return Ok(new
        {
            desc.ClientId,
            desc.ClientType,
            desc.DisplayName,
            RedirectUris = desc.RedirectUris.Select(u => u.ToString()),
            PostLogoutRedirectUris = desc.PostLogoutRedirectUris.Select(u => u.ToString()),
            AllowedScopes = scopes,
            Grants = new
            {
                AuthorizationCode = desc.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode),
                ClientCredentials = desc.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.ClientCredentials),
                RefreshToken = desc.Permissions.Contains(OpenIddictConstants.Permissions.GrantTypes.RefreshToken)
            },
            RequirePkce = desc.Requirements.Contains(OpenIddictConstants.Requirements.Features.ProofKeyForCodeExchange)
        });
    }

    [HttpDelete("{clientId}")]
    public async Task<IActionResult> Delete(string clientId)
    {
        var app = await _apps.FindByClientIdAsync(clientId);
        if (app is null) return NotFound();

        await _apps.DeleteAsync(app);
        return NoContent();
    }
}