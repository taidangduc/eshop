using EShop.Application.Order.Commands.SetPaymentRejectedOrderStatus;
using Ardalis.GuardClauses;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus.Abstractions;
using MediatR;

namespace EShop.Application.Order.IntegrationEventHandlers;

public class PaymentRejectedIntegrationEventHandler(IMediator mediator) 
    : IIntegrationEventHandler<PaymentRejectedIntegrationEvent>
{
    public async Task Handle(PaymentRejectedIntegrationEvent integrationEvent)
    {
        Guard.Against.Null(integrationEvent);

        var command = new SetPaymentRejectedOrderStatusCommand(integrationEvent.OrderNumber);

        await mediator.Send(command);
    }
}
