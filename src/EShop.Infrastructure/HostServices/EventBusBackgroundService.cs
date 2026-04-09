using EShop.EventBus;
using Microsoft.Extensions.Hosting;

namespace EShop.Infrastructure.HostServices;

public class EventBusBackgroundService<TConsumer, TEvent> : BackgroundService
    where TEvent : IntegrationEvent
{
    private readonly IEventBusConsumer<TEvent> _consumer;

    public EventBusBackgroundService(IEventBusConsumer<TEvent> consumer)
    {
        _consumer = consumer;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _consumer.ReceiveAsync(stoppingToken);
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}