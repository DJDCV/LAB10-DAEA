using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;

namespace Lab10.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());

        return services;
    }
}