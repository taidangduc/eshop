using EShop.Application.Order.Commands.SetConfirmedOrderStatus;
using Ardalis.GuardClauses;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus.Abstractions;
using MediatR;

namespace EShop.Application.Order.IntegrationEventHandlers;

public class PaymentSucceededIntegrationEventHandler(IMediator mediator) 
    : IIntegrationEventHandler<PaymentSucceededIntegrationEvent>
{
    public async Task Handle(PaymentSucceededIntegrationEvent integrationEvent)
    {
        Guard.Against.Null(integrationEvent);

        var command = new SetConfirmedOrderStatusCommand(integrationEvent.OrderNumber, integrationEvent.CardBrand, integrationEvent.TransactionId);

        await mediator.Send(command);
    }
}
