using MediatR;

namespace EShop.Application.Catalog.Products.Commands.DeleteVariant;

public record DeleteVariantCommand(Guid Id, Guid ProductId) : IRequest<Unit>;