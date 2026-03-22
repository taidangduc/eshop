using MediatR;

namespace EShop.Application.Order.Commands.SetCompletedOrderStatus;

public record SetCompletedOrderStatusCommand(Guid OrderId) : IRequest<bool>;
