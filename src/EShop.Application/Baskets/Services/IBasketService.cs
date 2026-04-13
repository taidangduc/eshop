using EShop.Application.Baskets.DTOs;

namespace EShop.Application.Baskets.Services;

public interface IBasketService
{
    Task<BasketDto> GetBasketAsync(Guid CustomerId);
    Task ClearBasketAsync(Guid CustomerId);
}

