using EShop.Domain.SeedWork;

namespace EShop.Domain.Entities;

public class ProductImage : Entity<Guid>
{
    public Guid ProductId { get; set; }
    public bool IsMain { get; set; }
    public string? ImageUrl { get; set; }
    public int SortOrder { get; set; }
}