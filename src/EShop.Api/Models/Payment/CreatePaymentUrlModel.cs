using EShop.Domain.Enums;

namespace EShop.Api.Models.Payment;

public record CreatePaymentUrlModel(long OrderNumber, decimal Amount, PaymentProvider Provider);
