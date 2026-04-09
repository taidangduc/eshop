using EShop.Application.Payments.Services;
using EShop.Domain.Enums;
using EShop.Infrastructure.Payment.Stripe;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure.Payment;

public class PaymentGatewayFactory : IPaymentGatewayFactory
{
    private readonly IServiceProvider _serviceProvider;

    public PaymentGatewayFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IPaymentGateway Resolve(PaymentProvider provider)
    {
        return provider switch
        {
            PaymentProvider.Stripe => _serviceProvider.GetRequiredService<StripePaymentGateway>(),
            _ => throw new NotSupportedException($"Payment provider '{provider}' is not supported.")
        };
    }
}