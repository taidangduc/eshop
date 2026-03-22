using EShop.Application.Order.Commands.SetProcessingOrderStatus;
using Ardalis.GuardClauses;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus.Abstractions;
using MediatR;

namespace EShop.Application.Order.IntegrationEventHandlers;

public class GracePeriodConfirmedIntegrationEventHandler(IMediator mediator)
    : IIntegrationEventHandler<GracePeriodConfirmedIntegrationEvent>
{
    public async Task Handle(GracePeriodConfirmedIntegrationEvent integrationEvent)
    {
        Guard.Against.Null(integrationEvent);

        var command = new SetProcessingOrderStatusCommand(integrationEvent.OrderId);

        await mediator.Send(command);
    }
}