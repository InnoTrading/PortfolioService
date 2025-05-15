using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Infrastructure.Data;
using PortfolioService.Infrastructure.Messaging;
using PortfolioService.Infrastructure.Messaging.Handlers;
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
        services
          .AddSingleton<IRpcQueueHandler, GetAvailableBalanceRpcHandler>()
          .AddSingleton<IRpcQueueHandler, StockCountRpcHandler>()
          .AddSingleton<IRpcQueueHandler, ReleaseReservedBalanceRpcHandler>()
          .AddSingleton<IRpcQueueHandler, ReserveBalanceRpcHandler>()
          .AddSingleton<IEventQueueHandler, OrderExecutedEventHandler>()
          .AddHostedService<RpcServer>()
          .AddHostedService<RabbitMqEventConsumer>();



        return services;
    }
}