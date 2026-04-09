namespace EShop.Application.Variants.DTOs;

// shared DTO
public class VariantSummary
{
    public decimal MinPrice { get; set; }
    public decimal MaxPrice { get; set; }
    public int TotalStock { get; set; }
}