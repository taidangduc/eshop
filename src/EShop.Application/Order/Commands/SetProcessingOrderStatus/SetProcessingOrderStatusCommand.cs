using MediatR;

namespace EShop.Application.Order.Commands.SetProcessingOrderStatus;

public record SetProcessingOrderStatusCommand(Guid OrderId) : IRequest<bool>;