using EShop.Application.Payments.Services;
using EShop.Domain.Enums;
using MediatR;

namespace EShop.Application.Payments.Commands;

public record HandleReturnCommand(
    PaymentProvider provider,
    IDictionary<string, string> parameters)
    : IRequest<PaymentResultResponse>;

internal class HandleReturnCommandHandler : IRequestHandler<HandleReturnCommand, PaymentResultResponse>
{
    private readonly IPaymentGatewayFactory _factory;

    public HandleReturnCommandHandler(IPaymentGatewayFactory factory)
    {
        _factory = factory;
    }

    // NOTE: 
    // This isn't [SOURCE OF TRUTH], so you should not update order status in this handler,
    // Update order status (business logic) should be process via webhook,
    public Task<PaymentResultResponse> Handle(HandleReturnCommand request, CancellationToken cancellationToken)
    {
        var gateway = _factory.Resolve(request.provider);
        var response = gateway.HandleReturnAsync(request.parameters);
        // TODO

        return response;
    }
}