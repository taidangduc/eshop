using EShop.Application.Payments.Services;
using EShop.Domain.Enums;
using EShop.Domain.Repositories;
using MediatR;

namespace EShop.Application.Payments.Commands;

public record HandleWebhookCommand(
    PaymentProvider provider,
    IDictionary<string, string> parameters)
    : IRequest<bool>;

internal class HandleWebhookCommandHandler : IRequestHandler<HandleWebhookCommand, bool>
{
    private readonly IPaymentGatewayFactory _factory;
    private readonly IOrderRepository _orderRepository;

    public HandleWebhookCommandHandler(IPaymentGatewayFactory factory, IOrderRepository orderRepository)
    {
        _factory = factory;
        _orderRepository = orderRepository;
    }

    // [Source of truth]
    public async Task<bool> Handle(HandleWebhookCommand request, CancellationToken cancellationToken)
    {
        var gateway = _factory.Resolve(request.provider);
        var response = await gateway.HandleWebhookAsync(request.parameters);

        var order = await _orderRepository.GetByOrderNumber(response.OrderNumber);

        if (order is null)
        {
            return false;
        }

        if (response.IsSuccess)
        {
            order.SetCompletedStatus();
        }
        else
        {
            order.SetRejectedStatusWhenPaymentRejected();
        }
        
        return await _orderRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}