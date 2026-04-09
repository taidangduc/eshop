using EShop.Application.Baskets.DTOs;
using EShop.Domain.Repositories;

namespace EShop.Application.Baskets.Services;

public class BasketService : IBasketService
{
    private readonly IBasketRepository _basketRepository;

    public BasketService(IBasketRepository basketRepository)
    {
        _basketRepository = basketRepository;
    }

    public async Task<BasketDto> GetBasketAsync(Guid CustomerId)
    {
        var basket = await _basketRepository.GetByCustomerIdWithItemsAsync(CustomerId);

        return basket is not null ? MapToBasketCheckoutDto(basket) : new();
    }

    private BasketDto MapToBasketCheckoutDto(Domain.Entities.Basket basket)
    {
        var response = new BasketDto { Id = basket.Id, Items = new List<BasketItemDto>() };

        var items = basket.Items.Select(item => new BasketItemDto
        {
            VariantId = item.VariantId,
            Quantity = item.Quantity
        }).ToList();

        response.Items.AddRange(items);

        return response;
    }
}