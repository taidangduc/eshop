namespace EShop.EventBus;

public interface IEventDispatcher
{
    Task DispatchAsync<T>(T message, CancellationToken cancellationToken = default);
}