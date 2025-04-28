using System;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Infrastructure.Messaging.Dtos;
using RabbitMQ.Client;

namespace PortfolioService.Infrastructure.Messaging.Handlers
{
    public class ReserveBalanceRpcHandler : IRpcQueueHandler
    {
        public string QueueName => "user_reserve_balance_rpc_queue";
        private readonly IServiceScopeFactory _scopeFactory;

        public ReserveBalanceRpcHandler(IServiceScopeFactory scopeFactory)
            => _scopeFactory = scopeFactory;

        public async Task<byte[]> HandleAsync(
            ReadOnlyMemory<byte> body,
            IReadOnlyBasicProperties props,
            IChannel channel,
            CancellationToken ct)
        {
            var json = Encoding.UTF8.GetString(body.Span);
            var req = JsonSerializer.Deserialize<ReserveBalanceRequestDto>(json)!;

            await using var scope = _scopeFactory.CreateAsyncScope();
            var svc = scope.ServiceProvider.GetRequiredService<IAccountManager>();

            var success = await svc.ReserveBalance(req.UserId, req.Amount);

            var response = success ? "true" : "false";
            return Encoding.UTF8.GetBytes(response);
        }
    }
}
