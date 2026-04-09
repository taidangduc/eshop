using EShop.Application.Payments.Services;
using EShop.Infrastructure.Payment;
using EShop.Infrastructure.Payment.Stripe;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure.ExternalServices.Payment;

public static class PaymentCollectionExtensions
{
    public static IServiceCollection AddPayment(this IServiceCollection services, PaymentOptions options)
    {
        services.AddScoped<IPaymentGateway, StripePaymentGateway>(x => new StripePaymentGateway(options.Stripe));
        services.AddScoped<IPaymentGatewayFactory, PaymentGatewayFactory>();

        return services;
    }
}