using EShop.EventBus.Abstractions;
using EShop.EventBus.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace EShop.EventBus.RabbitMQ;

public class RabbitMqReceiver : BackgroundService, IDisposable
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConnection _connection;
    private readonly RabbitMqOptions _options;
    private readonly EventBusSubscriptionManager _subscriptions;
    private readonly ILogger<RabbitMqReceiver> _logger;
    private IChannel? _channel;

    public RabbitMqReceiver(
        IServiceProvider serviceProvider,
        IConnection connection,
        IOptions<RabbitMqOptions> options,
        IOptions<EventBusSubscriptionManager> subscriptionManager,
        ILogger<RabbitMqReceiver> logger)
    {
        _serviceProvider = serviceProvider;
        _connection = connection;
        _options = options.Value;
        _subscriptions = subscriptionManager.Value;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _channel = await _connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.ExchangeDeclareAsync(
            exchange: _options.ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(
            queue: _options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        foreach (var (eventName, _) in _subscriptions.EventTypes)
        {
            await _channel.QueueBindAsync(
                queue: _options.QueueName,
                exchange: _options.ExchangeName,
                routingKey: eventName,
                cancellationToken: stoppingToken);

            _logger.LogInformation("Bound queue {QueueName} to exchange {ExchangeName} with routing key {RoutingKey}",
                _options.QueueName, _options.ExchangeName, eventName);
        }

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var eventName = ea.RoutingKey;
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            try
            {
                await ProcessEventAsync(eventName, message);
                await _channel.BasicAckAsync(ea.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing event {EventName}", eventName);
                await _channel.BasicNackAsync(ea.DeliveryTag, false, true);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: _options.QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);

        _logger.LogInformation("RabbitMQ receiver started listening on queue {QueueName}", _options.QueueName);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task ProcessEventAsync(string eventName, string message)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();

        if (!_subscriptions.EventTypes.TryGetValue(eventName, out var eventType))
        {
            _logger.LogWarning("No subscription for event: {EventName}", eventName);
            return;
        }

        var integrationEvent = JsonSerializer.Deserialize(message, eventType, _subscriptions.JsonOptions) as IntegrationEvent;

        if (integrationEvent is null)
        {
            _logger.LogError("Failed to deserialize event: {EventName}", eventName);
            return;
        }

        foreach (var handler in scope.ServiceProvider.GetKeyedServices<IIntegrationEventHandler>(eventType))
        {
            try
            {
                await handler.Handle(integrationEvent);             
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error handling event: {EventName}", eventName);
            }
        }
    }

    public override void Dispose()
    {
        _channel?.Dispose();
    }
}

