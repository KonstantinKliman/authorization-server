using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.BusinessLogic.Services;
using AuthorizationServer.BusinessLogic.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationServer.BusinessLogic;

public static class DependencyInjections
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserService, UserService>();

        services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
        
        return services;
    }
}