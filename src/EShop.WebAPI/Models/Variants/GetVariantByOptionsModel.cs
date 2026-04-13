namespace EShop.Api.Models.Variants;

public record GetVariantByOptionsModel(Guid ProductId, List<Guid> OptionValueIds);
