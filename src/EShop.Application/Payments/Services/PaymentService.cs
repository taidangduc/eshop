namespace EShop.Application.Payments.Services;

public class PaymentService : IPaymentService
{
    private readonly IPaymentGatewayFactory _factory;

    public PaymentService(IPaymentGatewayFactory factory)
    {
        _factory = factory;
    }

    public async Task<string?> CreatePaymentSessionAsync(CreatePaymentSession request, CancellationToken cancellationToken = default)
    {
        var gateway = _factory.Resolve(request.Provider);

        var response = await gateway.CreatePaymentAsync(new CreatePaymentRequest
        {
            OrderNumber = request.OrderNumber,
            Amount = request.Amount,
        });

        return response.Message;
    }
}