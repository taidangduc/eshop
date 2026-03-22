using MediatR;

namespace EShop.Application.Catalog.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<Unit>;