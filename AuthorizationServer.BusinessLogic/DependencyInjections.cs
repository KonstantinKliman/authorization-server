using AuthorizationServer.BusinessLogic.Configuration;
using AuthorizationServer.BusinessLogic.Interfaces.Services;
using AuthorizationServer.BusinessLogic.Services;
using AuthorizationServer.BusinessLogic.Validators;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationServer.BusinessLogic;

public static class DependencyInjections
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<JwtSettings>(configuration.GetSection("Jwt"));
        
        services.AddScoped<IPasswordService, PasswordService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IAdminAuthService, AdminAuthService>();

        services.AddValidatorsFromAssemblyContaining<CreateUserDtoValidator>();
        
        return services;
    }
}