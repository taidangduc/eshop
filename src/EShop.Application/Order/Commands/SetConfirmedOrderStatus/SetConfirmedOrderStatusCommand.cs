using MediatR;

namespace EShop.Application.Order.Commands.SetConfirmedOrderStatus;

public record SetConfirmedOrderStatusCommand(
    long OrderNumber,
    string? CardBrand,
    string? TransactionId) : IRequest<bool>;
