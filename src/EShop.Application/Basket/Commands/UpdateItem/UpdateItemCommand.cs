using MediatR;

namespace EShop.Application.Basket.Commands.UpdateItem;

public record UpdateItemCommand(Guid AccountId, Guid Id, int Quantity) : IRequest<Guid>;