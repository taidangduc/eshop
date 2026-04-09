namespace EShop.EventBus;

public interface IIntegrationEventHandler<T>
{
    Task HandleAsync(T message, CancellationToken cancellationToken = default);
}