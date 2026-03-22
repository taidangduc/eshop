using EShop.Application.Order.Commands.SetCompletedOrderStatus;
using Ardalis.GuardClauses;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus.Abstractions;
using MediatR;

namespace EShop.Application.Order.IntegrationEventHandlers;

public class ReserveStockSucceededIntegrationEventHandler(IMediator mediator) : IIntegrationEventHandler<ReserveStockSucceededIntegrationEvent>
{
    private readonly IMediator _mediator = mediator;
    public async Task Handle(ReserveStockSucceededIntegrationEvent integrationEvent)
    {
        Guard.Against.Null(integrationEvent);

        var command = new SetCompletedOrderStatusCommand(integrationEvent.OrderId);

        await _mediator.Send(command);
    }
}
