using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace PortfolioService.Infrastructure.Messaging
{
    public class RpcServer
    {
        private const string QueueName = "user_free_balance_rpc_queue";
        private readonly IConnectionFactory _factory = new ConnectionFactory { HostName = "localhost" };

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            var connection = await _factory.CreateConnectionAsync(cancellationToken);
            var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

            await channel.QueueDeclareAsync(
                queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += async (model, ea) =>
            {
                var props = ea.BasicProperties; 
                var replyProps = new BasicProperties
                {
                    CorrelationId = props?.CorrelationId
                };


                decimal response = decimal.MinValue;
                try
                {
                    var requestMessage = Encoding.UTF8.GetString(ea.Body.ToArray());
                    Console.WriteLine($" [.] Get request: {requestMessage}");

                    response = ProcessPortfolioRequest(requestMessage);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(" [!] Failed during execution: " + ex.Message);
                }
                finally
                {
                    var responseBytes = Encoding.UTF8.GetBytes(Convert.ToString(response, CultureInfo.InvariantCulture));
                    await channel.BasicPublishAsync(
                        exchange: "",
                        routingKey: props.ReplyTo, 
                        mandatory: false,
                        basicProperties: replyProps,
                        body: responseBytes,
                        cancellationToken: cancellationToken);

                    await channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: cancellationToken);
                }
            };

            await channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken);

            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
        private decimal ProcessPortfolioRequest(string userId)
        {
            return 0;
        }
    }
}
