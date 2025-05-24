using Microsoft.Extensions.DependencyInjection;
using Lab10.Domain.Interfaces;
using Lab10.Infrastructure.Persistence;

namespace Lab10.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}