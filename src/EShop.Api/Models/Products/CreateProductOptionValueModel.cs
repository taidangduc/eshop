namespace EShop.Api.Models.Products;

public record CreateProductOptionValueModel(Guid ProductId, Guid ProductOptionId, string Value, IFormFile? MediaFile = null);
