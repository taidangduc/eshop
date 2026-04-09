using EShop.Application.Orders.Commands;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus;
using MediatR;

namespace EShop.Infrastructure.IntegrationEventHandlers;

public class OrderConsumer(IMediator _mediator) :
    IIntegrationEventHandler<PaymentSucceedEvent>,
    IIntegrationEventHandler<PaymentFailedEvent>,
    IIntegrationEventHandler<StockDecreasedEvent>,
    IIntegrationEventHandler<StockDecreaseFailedEvent>,
    IIntegrationEventHandler<GracePeriodEvent>
{
    public async Task HandleAsync(PaymentSucceedEvent message, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new SetConfirmedOrderStatusCommand(message.OrderNumber, message.CardBrand, message.TransactionId), cancellationToken);
    }

    public async Task HandleAsync(PaymentFailedEvent message, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new SetPaymentRejectedOrderStatusCommand(message.OrderNumber), cancellationToken);
    }

    public async Task HandleAsync(StockDecreasedEvent message, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new SetCompletedOrderStatusCommand(message.OrderId), cancellationToken);
    }

    public async Task HandleAsync(StockDecreaseFailedEvent message, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new SetStockRejectedOrderStatusCommand(message.OrderId), cancellationToken);
    }

    public async Task HandleAsync(GracePeriodEvent message, CancellationToken cancellationToken = default)
    {
        await _mediator.Send(new SetProcessingOrderStatusCommand(message.OrderId), cancellationToken);
    }
}