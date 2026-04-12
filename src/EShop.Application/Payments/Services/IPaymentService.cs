using EShop.Domain.Enums;

namespace EShop.Application.Payments.Services;

public interface IPaymentService
{
    Task<string?> CreatePaymentSessionAsync(CreatePaymentSession request, CancellationToken cancellationToken = default);
}

public class CreatePaymentSession
{
    public long OrderNumber { get; set; }
    public decimal Amount { get; set; }
    public PaymentProvider Provider { get; set; }
}