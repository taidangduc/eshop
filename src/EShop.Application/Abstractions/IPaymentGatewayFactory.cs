using EShop.Domain.Enums;

namespace EShop.Application.Abstractions;

public interface IPaymentGatewayFactory
{
    IPaymentGateway Resolve(PaymentProvider provider);
}
