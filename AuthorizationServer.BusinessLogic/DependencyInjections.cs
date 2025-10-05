using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.BusinessLogic.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationServer.BusinessLogic;

public static class DependencyInjections
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserService, UserService>();
        
        return services;
    }
}