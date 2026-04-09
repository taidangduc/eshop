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

    // Create payment url for the order,
    // return the url to frontend, 
    // then frontend will redirect user to the url to complete payment
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