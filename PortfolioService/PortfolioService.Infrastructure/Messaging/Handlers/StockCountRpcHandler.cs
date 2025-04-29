using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Domain.Interfaces;
using PortfolioService.Infrastructure.Messaging.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace PortfolioService.Infrastructure.Messaging.Handlers
{
    public class StockCountRpcHandler : IRpcQueueHandler
    {
        public string QueueName => "user_stock_count_rpc_queue";
        private readonly IServiceScopeFactory _scopeFactory;

        public StockCountRpcHandler(IServiceScopeFactory scopeFactory)
            => _scopeFactory = scopeFactory;

        public async Task<byte[]> HandleAsync(
            ReadOnlyMemory<byte> body,
            IReadOnlyBasicProperties props,
            IChannel channel,
            CancellationToken ct)
        {
            var json = Encoding.UTF8.GetString(body.Span);
            var req = JsonSerializer.Deserialize<StockCountRequestDto>(json)!;

            await using var scope = _scopeFactory.CreateAsyncScope();
            var portfolioMgr = scope.ServiceProvider.GetRequiredService<IPortfolioManager>();

            var stocks = await portfolioMgr.GetStocks(req.UserId);
            var count = stocks.Where(s => s.StockTicker == req.StockTicker)
                               .Sum(s => s.Quantity);

            var resp = count.ToString();
            return Encoding.UTF8.GetBytes(resp);
        }
    }
}
