using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Infrastructure.Data;
using PortfolioService.Infrastructure.Messaging;
using PortfolioService.Infrastructure.Repositories;

namespace PortfolioService.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<PortfolioDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSingleton<RpcServer>(); 
        services.AddHostedService<RabbitMqBackgroundService>();


        return services;
    }
}