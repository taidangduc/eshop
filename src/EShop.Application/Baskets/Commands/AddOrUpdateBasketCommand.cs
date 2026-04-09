using EShop.Application.Variants.Services;
using EShop.Application.Customers.Services;
using EShop.Domain.Entities;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using MediatR;
using EShop.Domain.Identity;

namespace EShop.Application.Baskets.Commands;

public record AddOrUpdateBasketCommand(Guid Id, int Quantity) : IRequest<Guid>;

internal class AddOrUpdateBasketCommandHandler : IRequestHandler<AddOrUpdateBasketCommand, Guid>
{
    private readonly IBasketRepository _repository;
    private readonly ICustomerService _customerService;
    private readonly ICurrentUser _currentWebUser;
    private readonly IVariantService _variantService;

    public AddOrUpdateBasketCommandHandler(
        IBasketRepository repository,
        ICustomerService customerService,
        ICurrentUser currentWebUser,
        IVariantService variantService)
    {
        _repository = repository;
        _customerService = customerService;
        _currentWebUser = currentWebUser;
        _variantService = variantService;
    }

    public async Task<Guid> Handle(AddOrUpdateBasketCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetCustomerAsync(_currentWebUser.UserId);

        if (customer == null)
        {
            throw new NotFoundException("Customer not found");
        }

        var variant = await _variantService.GetVariantAsync(request.Id);

        if (variant == null)
        {
            throw new NotFoundException("Product variant not found");
        }

        if (request.Quantity < 0)
        {
            throw new ValidationException("Quantity must be greater than or equal to 0");
        }

        if (request.Quantity > variant.Quantity)
        {
            throw new ValidationException($"Quantity must be less than or equal to the available stock. Available stock: {variant.Quantity}");
        }

        var basket = await _repository.GetByCustomerIdWithItemsAsync(customer.Id, cancellationToken);

        if (basket == null)
        {
            basket = new Basket()
            {
                CustomerId = customer.Id,
                Items = new List<BasketItem>(),
                CreatedAt = DateTime.UtcNow,
            };
            await _repository.AddAsync(basket, cancellationToken);
        }

        var existingItem = basket.Items.FirstOrDefault(x => x.VariantId == request.Id);

        if (existingItem == null)
        {
            if (request.Quantity > 0)
            {
                basket.Items.Add(new BasketItem()
                {
                    VariantId = request.Id,
                    Quantity = request.Quantity,
                    CreatedAt = DateTime.UtcNow,
                });
            }
        }
        else
        {
            if (request.Quantity > 0)
            {
                existingItem.Quantity = request.Quantity;
            }
            else
            {
                basket.Items.Remove(existingItem);
            }
        }

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return basket.Id;
    }
}