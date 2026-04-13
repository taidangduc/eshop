namespace EShop.Api.Models.Products;

public record CreateProductOptionModel(Guid ProductId, string OptionName, bool HasImage = false);
