using MediatR;

namespace EShop.Application.Order.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : IRequest<bool>;
