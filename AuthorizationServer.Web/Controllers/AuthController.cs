using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.Web.Requests.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Web.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Логин пользователя и получение authorization code
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(
            request.Username, 
            request.Password, 
            request.ClientId, 
            request.RedirectUri);

        if (result.IsFailed)
            return BadRequest(new { error = result.Errors.First().Message });

        return Ok(result.Value);
    }

    /// <summary>
    /// Обмен authorization code на access и refresh токены
    /// </summary>
    [HttpPost("token")]
    public async Task<IActionResult> Token([FromBody] TokenRequest request)
    {
        if (request.GrantType == "authorization_code")
        {
            var result = await _authService.ExchangeCodeForTokensAsync(
                request.Code!, 
                request.ClientId, 
                request.ClientSecret!);

            if (result.IsFailed)
                return BadRequest(new { error = result.Errors.First().Message });

            return Ok(result.Value);
        }

        if (request.GrantType == "refresh_token")
        {
            var result = await _authService.RefreshTokenAsync(request.RefreshToken!);

            if (result.IsFailed)
                return BadRequest(new { error = result.Errors.First().Message });

            return Ok(result.Value);
        }

        return BadRequest(new { error = "Неподдерживаемый grant_type" });
    }

    /// <summary>
    /// Получение информации о текущем пользователе по токену
    /// </summary>
    [Authorize]
    [HttpGet("userinfo")]
    public async Task<IActionResult> UserInfo()
    {
        var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
        
        var result = await _authService.GetUserInfoAsync(token);

        if (result.IsFailed)
            return Unauthorized(new { error = result.Errors.First().Message });

        return Ok(result.Value);
    }

    /// <summary>
    /// Выход (отзыв refresh токена)
    /// </summary>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
    {
        await _authService.LogoutAsync(request.RefreshToken);
        return Ok(new { message = "Успешный выход" });
    }
}