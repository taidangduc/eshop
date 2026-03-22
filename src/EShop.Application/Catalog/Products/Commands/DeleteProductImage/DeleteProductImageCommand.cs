using MediatR;

namespace EShop.Application.Catalog.Products.Commands.DeleteProductImage;

public record DeleteProductImageCommand(Guid Id, Guid ProductId) : IRequest<Unit>;
