using Microsoft.Extensions.DependencyInjection;

namespace AuthorizationServer.BusinessLogic;

public static class DependencyInjections
{
    public static IServiceCollection AddBusinessLogicLayer(this IServiceCollection services)
    {
        return services;
    }
}