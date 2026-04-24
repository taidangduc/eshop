using EShop.Application.Orders.Services;
using EShop.Application.Payments.Services;
using EShop.Domain.Enums;
using EShop.Domain.Exceptions;
using MediatR;

namespace EShop.Application.Payments.Commands;

public record CreatePaymentCommand(
    Guid OrderId,
    PaymentProvider Provider)
    : IRequest<CreatePaymentResponse>;

internal class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, CreatePaymentResponse>
{
    private readonly IPaymentGatewayFactory _factory;
    private readonly IOrderService _orderService;

    public CreatePaymentCommandHandler(IPaymentGatewayFactory factory, IOrderService orderService)
    {
        _factory = factory;
        _orderService = orderService;
    }
    // NOTE: 
    // You should create payment url while in transaction of creating order, 
    // and if payment failed, you can cancel the order, this is more atomic and also can avoid creating payment url for non exist order.
    // OPTIONAL: 
    // I create payment url in a separate command handler just for demo purpose, 
    // to show how to create payment url without coupling with order creation.
    public async Task<CreatePaymentResponse> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetOrderSummaryAsync(request.OrderId, cancellationToken);

        if (order == null)
        {
            throw new NotFoundException("Order not found");
        }

        if (order.Status == OrderStatus.Completed)
        {
            return new CreatePaymentResponse
            {
                IsSuccess = false,
                Message = "Order already completed"
            };
        }

        var gateway = _factory.Resolve(request.Provider);

        var response = await gateway.CreatePaymentAsync(new CreatePaymentRequest
        {
            OrderNumber = order.OrderNumber,
            Amount = order.TotalAmount,
        });

        return response;
    }
}