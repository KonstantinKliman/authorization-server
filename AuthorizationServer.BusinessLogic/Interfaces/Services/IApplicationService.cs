using AuthorizationServer.DataAccess.Dtos.Application;
using FluentResults;

namespace AuthorizationServer.BusinessLogic.Interfaces.Services;

public interface IApplicationService
{
    Task<Result<ApplicationDto>> CreateApplication(CreateApplicationDto dto);
    Task<Result<ApplicationDto>> GetApplicationById(Guid id);
    Task<Result<List<ApplicationDto>>> GetAllApplications();
}