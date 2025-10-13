using System.Net;
using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.DataAccess.Dtos;
using AuthorizationServer.DataAccess.Dtos.Users;
using AuthorizationServer.Web.Requests.Admin.Users;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Web.Controllers.Admin;

[ApiController]
[Route("api/v1/admin/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var dto = request.Adapt<CreateUserDto>();
        var result = await _service.CreateUser(dto);
        
        // if (result.IsFailed) return BadRequest(new { message = result.Errors[0].Message });
        if (result.IsFailed)
        {
            var error = result.Errors[0];
        }

        return CreatedAtAction(nameof(GetUserById), new { id = result.Value.Id }, result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById([FromRoute] Guid id)
    {
        var result = await _service.GetUserById(id);
        
        if (result.IsFailed) return NotFound(new { message = result.Errors[0].Message });

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var result = await _service.GetAllUsers();
        return Ok(result.Value);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request, [FromRoute] Guid id)
    {
        var dto = request.Adapt<UpdateUserDto>();
        dto.Id = id;
        
        var result = await _service.UpdateUser(dto);

        if (result.IsFailed) return NotFound(new { message = result.Errors[0].Message });
        
        return Ok(result.Value);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var result = await _service.DeleteUser(id);
        if (result.IsFailed) return NotFound(new { message = result.Errors[0].Message });

        return Ok();
    }
}