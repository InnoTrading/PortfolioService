using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using PortfolioService.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text.Json.Serialization;
using System.Transactions;

namespace PortfolioService.Infrastructure.Messaging.Handlers
{

    public enum OperationType { Buy = 0, Sell = 1 }

    public record OrderExecutedEvent(
        Guid OrderId,
        string UserId,
        string Ticker,
        OperationType Operation,      
        decimal ExecutedPrice,
        int Amount,
        DateTime ExecutedAt,
        decimal PriceLimit
        );

    public class OrderExecutedEventHandler : IEventQueueHandler
    {
        public string QueueName => "order_executed_queue";
        private readonly IServiceScopeFactory _scopeFactory;

        public OrderExecutedEventHandler(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public async Task HandleAsync(ReadOnlyMemory<byte> body, IReadOnlyBasicProperties props, IChannel channel, ulong deliveryTag, CancellationToken ct)
        {
            try
            {
                var json = Encoding.UTF8.GetString(body.Span);
                var opts = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                var evt = JsonSerializer.Deserialize<OrderExecutedEvent>(json, opts)!;


                await using var scope = _scopeFactory.CreateAsyncScope();
                var accounts = scope.ServiceProvider.GetRequiredService<IAccountManager>();
                var portfolio = scope.ServiceProvider.GetRequiredService<IPortfolioManager>();

                var total = evt.Amount * evt.ExecutedPrice;

                if (evt.Operation == OperationType.Buy)
                {
                    using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                    try
                    {
                        await accounts.ReleaseReservedBalance(evt.UserId, evt.PriceLimit * evt.Amount);
                        await accounts.Withdrawal(evt.UserId, total);
                        await portfolio.AddStocks(evt.UserId, evt.Ticker, evt.Amount);

                        transaction.Complete();
                    }
                    catch
                    {
                        throw;
                    }
                }
                else if (evt.Operation == OperationType.Sell)
                {
                    using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                    try
                    {
                        await accounts.Deposit(evt.UserId, total);
                        await portfolio.RemoveStocks(evt.UserId, evt.Ticker, evt.Amount);

                        transaction.Complete();
                    }
                    catch
                    {
                        throw;
                    }
                }

                await channel.BasicAckAsync(deliveryTag, false, ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to process OrderExecutedEvent: {ex.Message}");
                await channel.BasicNackAsync(deliveryTag, false, true, ct);
            }
        }
    }
}
