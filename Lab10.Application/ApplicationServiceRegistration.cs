using Lab10.Application.Interfaces;
using Lab10.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Lab10.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<RoleService>();
        services.AddScoped<TicketService>();
        services.AddScoped<ResponseService>();

        return services;
    }
}