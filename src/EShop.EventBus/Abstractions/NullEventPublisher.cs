using EShop.EventBus.Events;
using Microsoft.Extensions.Logging;

namespace EShop.EventBus.Abstractions;

public sealed class NullEventPublisher : IEventPublisher
{
    public NullEventPublisher(ILogger<NullEventPublisher> logger)
    {
        logger.LogInformation("NullEventPublisher is used");
    }

    public Task PublishAsync<TEvent>(TEvent @event) where TEvent : IntegrationEvent
    {
        return Task.CompletedTask;  
    }
}
