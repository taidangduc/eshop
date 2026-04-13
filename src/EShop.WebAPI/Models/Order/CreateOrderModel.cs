using EShop.Domain.Enums;

namespace EShop.Api.Models.Order;

public record CreateOrderModel(
    Guid CustomerId,
    PaymentMethod Method,
    PaymentProvider Provider,
    string Street,
    string City,
    string ZipCode);
