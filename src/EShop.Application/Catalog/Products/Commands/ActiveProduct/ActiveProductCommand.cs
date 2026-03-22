using MediatR;

namespace EShop.Application.Catalog.Products.Commands.ActiveProduct;

public record ActiveProductCommand(Guid ProductId, bool IsActive) : IRequest<Unit>;
