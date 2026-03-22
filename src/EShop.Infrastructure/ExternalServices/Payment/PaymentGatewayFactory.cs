using EShop.Application.Abstractions;
using EShop.Domain.Enums;
using EShop.Infrastructure.ExternalServices.Payment.Stripe;
using EShop.Infrastructure.ExternalServices.Payment.Vnpay;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure.ExternalServices.Payment;

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
            PaymentProvider.Vnpay => _serviceProvider.GetRequiredService<VnpayPaymentGateway>(),
            PaymentProvider.Stripe => _serviceProvider.GetRequiredService<StripePaymentGateway>(),
            _ => throw new NotSupportedException()
        };
    }
}