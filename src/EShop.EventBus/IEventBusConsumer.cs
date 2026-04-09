namespace EShop.EventBus;

public interface IEventBusConsumer
{
    Task ReceiveAsync(CancellationToken cancellationToken = default);
}

public interface IEventBusConsumer<T> : IEventBusConsumer
{
}