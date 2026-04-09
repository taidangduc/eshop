using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EShop.EventBus.RabbitMQ;

public sealed class RabbitMQSender<T> : IEventBusProducer<T>
{
    private readonly RabbitMQSenderOptions _options;
    private readonly ConnectionFactory _connectionFactory;
    private readonly string _exchangeName;
    private readonly string _routingKey;

    public RabbitMQSender(RabbitMQSenderOptions options)
    {
        _options = options;

        _connectionFactory = new ConnectionFactory
        {
            HostName = options.HostName,
            Port = options.Port,
            UserName = options.UserName,
            Password = options.Password,
        };

        _exchangeName = options.ExchangeName;
        _routingKey = options.RoutingKey;
    }

    public async Task SendAsync(T message, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync(cancellationToken);
        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.ExchangeDeclareAsync(_options.ExchangeName, _options.ExchangeType, durable: true, autoDelete: false, cancellationToken: cancellationToken);

        var dataJson = JsonSerializer.Serialize(message, message.GetType());
        var body = Encoding.UTF8.GetBytes(dataJson);

        var properties = new BasicProperties
        {
            Persistent = true,
        };

        await channel.BasicPublishAsync(_exchangeName, _routingKey, true, properties, body, cancellationToken);
    }
}

