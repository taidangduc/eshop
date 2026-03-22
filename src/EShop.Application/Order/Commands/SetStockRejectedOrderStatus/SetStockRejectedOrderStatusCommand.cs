using MediatR;

namespace EShop.Application.Order.Commands.SetStockRejectedOrderStatus;

public record SetStockRejectedOrderStatusCommand(Guid OrderId) : IRequest<bool>;
