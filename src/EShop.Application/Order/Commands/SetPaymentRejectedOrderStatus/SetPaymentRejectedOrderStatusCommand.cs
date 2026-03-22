using MediatR;

namespace EShop.Application.Order.Commands.SetPaymentRejectedOrderStatus;

public record SetPaymentRejectedOrderStatusCommand(long OrderNumber) : IRequest<bool>;
