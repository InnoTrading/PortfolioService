using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace PortfolioService.Infrastructure.Messaging
{
    public class RabbitMqBackgroundService(RpcServer rpcServer, ILogger<RabbitMqBackgroundService> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Starting RabbitMQ RPC Server...");
            await rpcServer.StartAsync(stoppingToken);
        }
    }
}