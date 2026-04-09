namespace EShop.EventBus;

public interface IEventBusProducer<T>
{
    Task SendAsync(T message, CancellationToken cancellationToken = default);
}