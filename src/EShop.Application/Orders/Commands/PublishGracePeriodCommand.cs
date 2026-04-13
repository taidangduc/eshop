using EShop.Domain.Repositories;
using MediatR;

namespace EShop.Application.Orders.Commands;

public record PublishGracePeriodCommand() : IRequest<Unit>;

internal class PublishGracePeriodCommandHandler : IRequestHandler<PublishGracePeriodCommand, Unit>
{
    private readonly IOrderRepository _orderRepository;
    public PublishGracePeriodCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Unit> Handle(PublishGracePeriodCommand request, CancellationToken cancellationToken)
    {
        // In real, you should use grace period for case like payment, 
        // but for demo, we just set processing status for orders in grace period.

        var orderIds = await _orderRepository.GetConfirmedGracePeriodOrdersAsync(cancellationToken);
        foreach (var orderId in orderIds)
        {
            var order = await _orderRepository.GetAsync(orderId, cancellationToken);

            if (order is null)
            {
                continue;
            }

            order.SetProcessingStatus();
        }

        await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return Unit.Value;
    }
}