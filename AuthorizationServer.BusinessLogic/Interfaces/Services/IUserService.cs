using AuthorizationServer.DataAccess.Dtos;
using FluentResults;

namespace AuthorizationServer.BusinessLogic.Interfaces.Services;

public interface IUserService
{
    Task<Result<UserDto>> CreateUser(UserDto dto);
    Task<Result<UserDto>> GetUserById(Guid id);
    Task<Result<List<UserDto>>> GetAllUsers();
    Task<Result<UserDto>> UpdateUser(UserDto dto);
    Task<Result> DeleteUser(Guid id);
}