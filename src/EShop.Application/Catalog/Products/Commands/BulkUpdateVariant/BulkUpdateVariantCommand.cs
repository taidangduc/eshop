using MediatR;

namespace EShop.Application.Catalog.Products.Commands.BulkUpdateVariant;

public record BulkUpdateVariantCommand(Guid ProductId, decimal? Price, int? Quantity, string? Sku, bool IsActive) : IRequest<Guid>;
