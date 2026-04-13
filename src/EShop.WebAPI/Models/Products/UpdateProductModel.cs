namespace EShop.Api.Models.Products;

public record UpdateProductModel(Guid Id, Guid CategoryId, string Title, string Description);
