using RabbitMQ.Client;

public interface IRpcQueueHandler
{
    string QueueName { get; }

    Task<byte[]> HandleAsync(
        ReadOnlyMemory<byte> body,
        IReadOnlyBasicProperties props,
        IChannel channel,
        CancellationToken cancellationToken);
}
