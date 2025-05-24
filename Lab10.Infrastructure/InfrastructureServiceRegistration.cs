using Lab10.Domain.Interfaces;
using Lab10.Infrastructure.Context;
using Lab10.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lab10.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<Lab10DbContext>(options =>
            options.UseMySql(configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        
       services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

