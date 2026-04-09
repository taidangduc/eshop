namespace EShop.Api.Models.Variants;

public record BulkUpdateVariantModel(Guid ProductId, decimal? Price, int? Quantity, string? Sku);
