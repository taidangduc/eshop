using EShop.Domain.SeedWork;

namespace EShop.Domain.Entities;

public class ProductOption : Entity<Guid>
{
    public Guid ProductId { get; set; }
    public string Name { get; set; }
    public bool HasImage { get; set; }
    private readonly List<ProductOptionValue> _values = new();
    public IReadOnlyCollection<ProductOptionValue> Values => _values.AsReadOnly();

    public void AddValue(string value, string? fileLocation = null)
    {
        _values.Add(new ProductOptionValue(Id, value, fileLocation));
    }

    public void RemoveValue(Guid valueId)
    {
        var value = _values.FirstOrDefault(v => v.Id == valueId);
        if (value is null)
        {
            throw new InvalidOperationException($"Option value with ID {valueId} not found.");
        }

        _values.Remove(value);
    }
}