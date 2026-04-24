using EShop.Domain.Enums;

namespace EShop.Api.Models.Payment;

public record CreatePaymentUrlModel(Guid OrderId, PaymentProvider Provider);
