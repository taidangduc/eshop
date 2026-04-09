namespace EShop.EventBus;

public interface IEventBus
{
    Task SendAsync<T>(T message, CancellationToken cancellationToken = default)
        where T : EventBusMessage;
}

public abstract class EventBusMessage
{
}

public abstract class IntegrationEvent : EventBusMessage
{
}