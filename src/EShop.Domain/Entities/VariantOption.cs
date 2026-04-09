namespace EShop.Domain.Entities;

/// <summary>
/// Snapshot of a product option value for a specific variant.
/// This allows us to avoid having to join with product options and option values when displaying variant details.
/// </summary>
public class VariantOption
{
    public Guid VariantId { get; set; }
    public Guid OptionId { get; set; }
    public Guid OptionValueId { get; set; }
    public string? OptionName { get; set; }
    public string? OptionValueName { get; set; }
}
