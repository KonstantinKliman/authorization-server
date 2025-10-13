using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.DataAccess.Dtos.Application;
using AuthorizationServer.Web.Requests.Admin.Applications;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationServer.Web.Controllers.Admin;

[ApiController]
[Route("api/v1/admin/applications")]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _service;

    public ApplicationController(IApplicationService service)
    {
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationRequest request)
    {
        var dto = request.Adapt<CreateApplicationDto>();
        var result = await _service.CreateApplication(dto);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetApplicationById([FromRoute] Guid id)
    {
        var result = await _service.GetApplicationById(id);
        if (result.IsFailed)
            return NotFound(new { message = result.Errors[0].Message });

        return Ok(result.Value);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllApplications()
    {
        var result = await _service.GetAllApplications();
        return Ok(result.Value);
    }
}