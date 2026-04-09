namespace EShop.Application.Variants.DTOs;

public class VariantOverview
{
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public int TotalStock { get; set; }
    public List<VariantDto> Variants { get; set; }
}