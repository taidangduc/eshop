namespace EShop.Api.Models.Products;

public record CreateProductModel(Guid CategoryId, string Name, string Description);
