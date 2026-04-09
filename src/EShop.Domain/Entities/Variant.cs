using EShop.Domain.Enums;
using EShop.Domain.SeedWork;

namespace EShop.Domain.Entities;

public class Variant : Entity<Guid>, IAggregateRoot, ISoftDelete
{
    public Guid ProductId { get; set; }
    public string? Title { get; set; }
    public string? Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Sku { get; set; }
    /// <summary>
    /// optional image URL for the variant, if the product option allows images.
    /// This is a snapshot of the product option value image URL at the time of variant creation,
    /// to avoid having to join with product options and option values when displaying variant details.
    /// </summary>
    public string? ImageUrl { get; set; }
    public VariantStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    private readonly List<VariantOption> _optionValues = new();
    public IReadOnlyCollection<VariantOption> OptionValues => _optionValues.AsReadOnly();

    public static Variant Create(Guid productId, string? title, decimal price, int quantity)
    {
        var variant = new Variant
        {
            Id = Guid.CreateVersion7(),
            ProductId = productId,
            Title = title,
            Price = price,
            Quantity = quantity,
            Status = VariantStatus.Active
        };

        return variant;

    }

    public void SetOptionValues(List<VariantOption> optionValues)
    {
        _optionValues.Clear();
        _optionValues.AddRange(optionValues);
    }

    public void ReserveStock(int quantity)
    {
        if (Quantity < quantity)
            throw new ArgumentException("Not enough quantity", nameof(quantity));

        if (Status != VariantStatus.Active)
            throw new InvalidOperationException("Variant is not active.");

        Quantity -= quantity;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}
