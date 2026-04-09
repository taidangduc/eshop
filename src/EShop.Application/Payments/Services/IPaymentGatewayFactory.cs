using EShop.Domain.Enums;

namespace EShop.Application.Payments.Services;

public interface IPaymentGatewayFactory
{
    IPaymentGateway Resolve(PaymentProvider provider);
}
