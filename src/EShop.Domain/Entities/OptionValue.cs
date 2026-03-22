using EShop.Domain.SeedWork;
using System.Text.Json.Serialization;

namespace EShop.Domain.Entities;

public class OptionValue : Entity<Guid>
{
    //public Guid Id { get; set; }
    public Guid OptionId { get; set; }
    public string Value { get; set; } = default!;
    public Guid? ImageId { get; set; }
    [JsonIgnore]
    public Image? Image { get; set; }
}