using EShop.EventBus.Events;

namespace EShop.EventBus.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : IntegrationEvent;
}
