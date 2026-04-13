namespace EShop.Api.Models.Variants;

public record UpdateVariantModel(Guid ProductId, Guid Id, decimal RegularPrice, int Quantity);
