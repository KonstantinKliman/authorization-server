using AuthorizationServer.DataAccess.Dtos;
using AuthorizationServer.DataAccess.Dtos.Users;
using FluentResults;

namespace AuthorizationServer.BusinessLogic.Interfaces.Services;

public interface IUserService
{
    Task<Result<UserDto>> CreateUser(CreateUserDto dto);
    Task<Result<UserDto>> GetUserById(Guid id);
    Task<Result<List<UserDto>>> GetAllUsers();
    Task<Result<UserDto>> UpdateUser(UpdateUserDto dto);
    Task<Result> DeleteUser(Guid id);
}