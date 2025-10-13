using AuthorizationServer.DataAccess.Dtos.Auth;
using FluentResults;

namespace AuthorizationServer.BusinessLogic.Interfaces.Services;

public interface IAdminAuthService
{
    Task<Result<TokenDto>> Login(string login, string password);
}