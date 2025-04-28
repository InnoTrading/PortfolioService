using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Domain.Interfaces;
using RabbitMQ.Client;                         

namespace PortfolioService.Infrastructure.Messaging.Handlers
{
    public class GetAvailableBalanceRpcHandler : IRpcQueueHandler
    {
        public string QueueName => "user_available_balance_rpc_queue";
        private readonly IServiceScopeFactory _scopeFactory;

        public GetAvailableBalanceRpcHandler(IServiceScopeFactory scopeFactory)
            => _scopeFactory = scopeFactory;

        public async Task<byte[]> HandleAsync(
            ReadOnlyMemory<byte> body,
            IReadOnlyBasicProperties props,
            IChannel channel,
            CancellationToken ct)
        {
            var userId = Encoding.UTF8.GetString(body.Span);

            await using var scope = _scopeFactory.CreateAsyncScope();
            var svc = scope.ServiceProvider.GetRequiredService<IAccountManager>();

            var balance = await svc.Balance(userId);
            var reserved = await svc.ReservedBalance(userId);
            var available = balance - reserved;

            return Encoding.UTF8.GetBytes(available.ToString(System.Globalization.CultureInfo.InvariantCulture));

        }
    }
}
