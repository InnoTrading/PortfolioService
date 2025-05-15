using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace PortfolioService.Infrastructure.Messaging
{
    public sealed class RpcServer : BackgroundService
    {
        private readonly ConnectionFactory _factory = new() { HostName = "localhost" };
        private readonly ILogger<RpcServer> _logger;
        private readonly IEnumerable<IRpcQueueHandler> _handlers;

        public RpcServer(IEnumerable<IRpcQueueHandler> handlers, ILogger<RpcServer> logger)
        {
            _handlers = handlers;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("RPC Server starting…");
            await using var conn = await _factory.CreateConnectionAsync(stoppingToken);
            await using var channel = await conn.CreateChannelAsync(cancellationToken: stoppingToken);

            await channel.BasicQosAsync(0, 1, false, stoppingToken);

            foreach (var h in _handlers)
            {
                await channel.QueueDeclareAsync(
                    queue: h.QueueName,
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null,
                    passive: false,
                    noWait: false,
                    cancellationToken: stoppingToken);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.ReceivedAsync += async (_, ea) =>
                {
                    var reqProps = ea.BasicProperties;

                    // RabbitMQ.Client 7.x: CreateBasicProperties() 
                    var replyProps = new BasicProperties
                    {
                        CorrelationId = reqProps.CorrelationId
                    };

                    byte[] resp;
                    try
                    {
                        resp = await h.HandleAsync(ea.Body, reqProps, channel, stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Handler {Q} failed", h.QueueName);
                        resp = Encoding.UTF8.GetBytes("error");
                    }

                    await channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: reqProps.ReplyTo,
                        mandatory: false,
                        basicProperties: replyProps,
                        body: resp,
                        cancellationToken: stoppingToken);
                    _logger.LogInformation("Started RPC consumer on queue {QueueName}", h.QueueName);

                    await channel.BasicAckAsync(ea.DeliveryTag, false, stoppingToken);
                };

                await channel.BasicConsumeAsync(
                    queue: h.QueueName,
                    autoAck: false,
                    consumer: consumer,
                    cancellationToken: stoppingToken);
            }

            _logger.LogInformation("RPC Server ready");
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}