using EShop.Application.Order.Commands.SetStockRejectedOrderStatus;
using Ardalis.GuardClauses;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus.Abstractions;
using MediatR;

namespace EShop.Application.Order.IntegrationEventHandlers;

public class ReserveStockRejectedIntegrationEventHandler(IMediator mediator) 
    : IIntegrationEventHandler<ReserveStockRejectedIntegrationEvent>
{
    public async Task Handle(ReserveStockRejectedIntegrationEvent integrationEvent)
    {
        Guard.Against.Null(integrationEvent);

        var command = new SetStockRejectedOrderStatusCommand(integrationEvent.OrderId);

        await mediator.Send(integrationEvent);
    }
}
