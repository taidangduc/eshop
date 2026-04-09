using EShop.Application.Baskets.Commands;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus;
using MediatR;

namespace EShop.Infrastructure.IntegrationEventHandlers;

public class BasketConsumer(IMediator _mediator) : IIntegrationEventHandler<OrderCreatedEvent>
{
    public async Task HandleAsync(OrderCreatedEvent message, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new ClearBasketCommand(message.CustomerId), cancellationToken);
    }
}