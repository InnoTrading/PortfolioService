using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Infrastructure.Messaging.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PortfolioService.Infrastructure.Messaging.Handlers
{
    public class ReleaseReservedBalanceRpcHandler : IRpcQueueHandler
    {
        public string QueueName => "user_release_reserved_balance_rpc_queue";
        private readonly IServiceScopeFactory _scopeFactory;

        public ReleaseReservedBalanceRpcHandler(IServiceScopeFactory scopeFactory)
            => _scopeFactory = scopeFactory;

        public async Task<byte[]> HandleAsync(
            ReadOnlyMemory<byte> body,
            IReadOnlyBasicProperties props,
            IChannel channel,
            CancellationToken ct)
        {
            var json = Encoding.UTF8.GetString(body.Span);
            var req = JsonSerializer.Deserialize<ReleaseReservedBalanceRequestDto>(json)!;

            await using var scope = _scopeFactory.CreateAsyncScope();
            var svc = scope.ServiceProvider.GetRequiredService<IAccountManager>();

            var success = await svc.ReleaseReservedBalance(req.UserId, req.Amount);

            var response = success ? "true" : "false";
            return Encoding.UTF8.GetBytes(response);
        }
    }
}
