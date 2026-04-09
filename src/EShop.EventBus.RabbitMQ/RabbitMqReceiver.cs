using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EShop.EventBus.RabbitMQ;

public class RabbitMQReceiver<T> : IEventBusConsumer<T>, IDisposable
{
    private readonly RabbitMQReceiverOptions _options;
    private readonly IEventDispatcher _dispatcher;
    private IConnection _connection;
    private IChannel _channel;

    public RabbitMQReceiver(RabbitMQReceiverOptions options, IEventDispatcher dispatcher)
    {
        _options = options;
        _dispatcher = dispatcher;
    }

    public async Task ReceiveAsync(CancellationToken cancellationToken)
    {
        _connection = await new ConnectionFactory
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password,
            AutomaticRecoveryEnabled = true,
        }.CreateConnectionAsync(cancellationToken);

        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        _connection.ConnectionShutdownAsync += (sender, args) =>
        {
            // Handle connection shutdown if needed
            return Task.CompletedTask;
        };

        if (_options.AutomaticCreateEnabled)
        {
            await _channel.ExchangeDeclareAsync(_options.ExchangeName, _options.ExchangeType, true, false, null, cancellationToken: cancellationToken);

            await _channel.QueueDeclareAsync(_options.QueueName, true, false, false, null, cancellationToken: cancellationToken);

            await _channel.QueueBindAsync(_options.QueueName, _options.ExchangeName, _options.RoutingKey, null, cancellationToken: cancellationToken);
        }

        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false, cancellationToken: cancellationToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return;
            }

            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var message = JsonSerializer.Deserialize<T>(json);

                if (message is null)
                {
                    await _channel.BasicRejectAsync(ea.DeliveryTag, false);
                    return;
                }

                await _dispatcher.DispatchAsync(message, cancellationToken: cancellationToken);
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch
            {
                await _channel.BasicRejectAsync(ea.DeliveryTag, false);
            }
        };

        await _channel.BasicConsumeAsync(_options.QueueName, false, consumer, cancellationToken: cancellationToken);
    }

    public void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
    }
}

