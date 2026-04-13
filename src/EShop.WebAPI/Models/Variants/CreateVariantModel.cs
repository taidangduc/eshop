namespace EShop.Api.Models.Variants;

public record CreateVariantModel(Guid ProductId, decimal RegularPrice, int Quantity);
