using EShop.Domain.SeedWork;

namespace EShop.Domain.Entities;

public class ProductOptionValue : Entity<Guid>
{
    /// <summary>
    /// Parameterless constructor required by EF Core for entity materialization.
    /// </summary>
    protected ProductOptionValue() { }

    public ProductOptionValue(Guid optionId, string value, string? imageUrl = null)
    {
        OptionId = optionId;
        Name = value;
        ImageUrl = imageUrl;
    }

    public Guid OptionId { get; set; }
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
}