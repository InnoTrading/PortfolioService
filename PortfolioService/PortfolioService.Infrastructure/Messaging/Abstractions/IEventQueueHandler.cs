using RabbitMQ.Client;

public interface IEventQueueHandler
{
    string QueueName { get; }

    Task HandleAsync(
        ReadOnlyMemory<byte> body,
        IReadOnlyBasicProperties props,
        IChannel channel,
        ulong deliveryTag,
        CancellationToken cancellationToken);
}
