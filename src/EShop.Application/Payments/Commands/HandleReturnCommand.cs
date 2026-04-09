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

    // In payment, HandleReturn is mean handle the return url after user complete payment in third party payment page,
    // we will get the payment result from third party payment provider, then update order status in our system, 
    // and return the payment result to frontend, frontend will show the payment result to user
    // *DON'T update order status to completed in this handler,
    // we will update order status in a separate handler which handle the webhook event from third party payment provider,
    // because "webhook is more reliable than return url and it's [source of truth]"
    // *Note: the return url is different from webhook url, 
    // return url is for redirecting user after payment, 
    // webhook url is for receiving payment result from third party payment provider
    public Task<PaymentResultResponse> Handle(HandleReturnCommand request, CancellationToken cancellationToken)
    {
        var gateway = _factory.Resolve(request.provider);
        var response = gateway.HandleReturnAsync(request.parameters);

        return response;
    }
}