using EShop.EventBus.Abstractions;
using EShop.EventBus.Events;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace EShop.EventBus.RabbitMQ;

public sealed class RabbitMqSender : IEventPublisher
{
    private readonly RabbitMqOptions _options;
    private readonly ILogger<RabbitMqSender> _logger;
    private IConnection _connection;

    public RabbitMqSender(
        IConnection connection,
        IOptions<RabbitMqOptions> options,
        ILogger<RabbitMqSender> logger)
    {
        _connection = connection;
        _options = options.Value;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : IntegrationEvent
    {
        using var channel = (await _connection?.CreateChannelAsync()) ?? throw new InvalidOperationException("RabbitMQ connection is not open");

        await channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false);

        var eventName = @event.GetType().FullName!;
        var json = JsonSerializer.Serialize(@event, @event.GetType());
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json"
        };

        try
        {
            await channel.BasicPublishAsync(
                exchange: _options.ExchangeName,
                routingKey: eventName,
                mandatory: true,
                basicProperties: properties,
                body: body);

            _logger.LogInformation("Published event {EventName} to RabbitMQ", eventName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing event {EventName}", eventName);
            throw;
        }
    }
}

