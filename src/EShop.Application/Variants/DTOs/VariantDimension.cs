namespace EShop.Application.Variants.DTOs;

public class VariantDimension
{
    public Guid OptionId { get; set; }
    public Guid OptionValueId { get; set; }
    public bool HasImage { get; set; }
    public string? OptionName { get; set; }
    public string? OptionValueName { get; set; }
    public string? ImageUrl { get; set; }
}