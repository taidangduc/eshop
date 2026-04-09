namespace EShop.Application.Baskets.DTOs;

public class BasketDto
{
    public Guid Id { get; set;} 
    public List<BasketItemDto> Items { get; set;} 
}