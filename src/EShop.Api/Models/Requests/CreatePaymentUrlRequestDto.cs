using EShop.Domain.Enums;

namespace EShop.Api.Models.Requests;

public record CreatePaymentUrlRequestDto(long OrderNumber, decimal Amount, PaymentProvider Provider);
