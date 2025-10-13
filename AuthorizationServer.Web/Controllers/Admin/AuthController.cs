using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.Web.Requests.Admin.Auth;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Web.Controllers.Admin;

[ApiController]
[Route("/api/v1/admin/auth")]
public class AuthController : ControllerBase
{
    private readonly IAdminAuthService _authService;

    public AuthController(IAdminAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.Login(request.Username, request.Password);
        
        if (result.IsFailed)
            return BadRequest(new { error = result.Errors.First().Message });
        
        return Ok(result.Value);
    }
}