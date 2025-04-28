using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PortfolioService.Infrastructure.Messaging
{
    public class RabbitMqEventConsumer : BackgroundService
    {
        private readonly IEnumerable<IEventQueueHandler> _handlers;
        private readonly ConnectionFactory _factory;

        public RabbitMqEventConsumer(IEnumerable<IEventQueueHandler> handlers)
        {
            _handlers = handlers;
            _factory = new ConnectionFactory { HostName = "localhost" };
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _factory.CreateConnectionAsync();

            foreach (var handler in _handlers)
            {
                var channel = await connection.CreateChannelAsync();

                await channel.QueueDeclareAsync(
                    queue: handler.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    passive: false,
                    noWait: false,
                    cancellationToken: stoppingToken
                );

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (model, ea) =>
                {
                    await handler.HandleAsync(ea.Body, ea.BasicProperties, channel, ea.DeliveryTag, stoppingToken);
                };

                await channel.BasicConsumeAsync(
                    queue: handler.QueueName,
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: stoppingToken
                );
            }

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
