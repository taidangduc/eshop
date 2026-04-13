using EShop.Application.Baskets.DTOs;
using EShop.Application.Variants.Services;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using MediatR;
using EShop.Domain.Identity;
using EShop.Contracts.Customer.Services;

namespace EShop.Application.Baskets.Queries;

public record GetBasketQuery : IRequest<BasketOverview>;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, BasketOverview>
{
    private readonly IBasketRepository _basketRepository;
    private readonly ICurrentUser _currentWebUser;
    private readonly ICustomerService _customerService;
    private readonly IVariantService _variantService;

    public GetBasketQueryHandler(
        IBasketRepository basketRepository,
        ICurrentUser currentWebUser,
        ICustomerService customerService,
        IVariantService variantService)
    {
        _basketRepository = basketRepository;
        _currentWebUser = currentWebUser;
        _customerService = customerService;
        _variantService = variantService;
    }

    public async Task<BasketOverview> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetAsync(_currentWebUser.UserId);

        if (customer == null)
        {
            throw new NotFoundException("Customer not found");
        }

        var basket = await _basketRepository.GetByCustomerIdWithItemsAsync(customer.Id, cancellationToken);

        // If basket doesn't exist, return an empty basket
        if (basket == null)
        {
            return new BasketOverview();
        }

        BasketOverview basketDto = new BasketOverview
        {
            Id = basket.Id,
            CustomerId = customer.Id,
            CreatedAt = basket.CreatedAt,
            UpdatedAt = basket.UpdatedAt
        };

        // For display UI, entire ordered items in basket
        var basketItems = basket.Items.OrderBy(x => x.CreatedAt).ToList();

        var response = await _variantService.GetVariantsByIdsAsync(basketItems.Select(x => x.VariantId).ToList(), cancellationToken);

        var variantByIds = response?.ToDictionary(x => x.Id) ?? [];

        List<BasketItemOverview> items = basketItems.Select(item =>
        {
            variantByIds.TryGetValue(item.VariantId, out var variant);

            if (variant == default)
            {
                throw new ValidationException($"Product variant with id {item.VariantId} not found");
            }

            return new BasketItemOverview
            {
                VariantId = item.VariantId,
                Title = variant.Title,
                Name = variant.Name,
                Price = variant.Price,
                Quantity = item.Quantity,
                ImageUrl = variant.ImageUrl ?? string.Empty
            };
        }).ToList();

        basketDto.Items.AddRange(items);

        return basketDto;
    }
}
