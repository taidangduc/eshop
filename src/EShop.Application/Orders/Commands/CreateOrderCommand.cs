using EShop.Application.Baskets.Services;
using EShop.Application.Orders.DTOs;
using EShop.Application.Payments.Services;
using EShop.Application.Variants.Services;
using EShop.Domain.Entities;
using EShop.Domain.Enums;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using EShop.Domain.ValueObject;
using FluentValidation;
using MediatR;

namespace EShop.Application.Orders.Commands;

public record CreateOrderCommand(
    Guid CustomerId,
    PaymentMethod Method,
    PaymentProvider Provider,
    string Street,
    string City,
    string ZipCode) : IRequest<PlaceOrderDto>;

internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, PlaceOrderDto>
{
    private readonly IOrderRepository _orderRepository;
    private readonly IVariantService _variantService;
    private readonly IBasketService _basketService;
    private readonly IPaymentService _paymentService;

    public CreateOrderCommandHandler(
        IOrderRepository orderRepository,
        IVariantService variantService,
        IBasketService basketService,
        IPaymentService paymentService)
    {
        _orderRepository = orderRepository;
        _variantService = variantService;
        _basketService = basketService;
        _paymentService = paymentService;
    }

    public async Task<PlaceOrderDto> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        var basketItems = await _basketService.GetBasketAsync(command.CustomerId);

        var response = await _variantService.GetVariantsByIdsAsync(basketItems.Items.Select(x => x.VariantId).ToList());

        var variantByIds = response?.ToDictionary(x => x.Id) ?? [];

        List<OrderItem> orderItems = basketItems.Items.Select(basketItem =>
        {
            if (!variantByIds.TryGetValue(basketItem.VariantId, out var variant))
            {
                throw new NotFoundException();
            }

            if (variant.Quantity < basketItem.Quantity)
            {
                throw new NotFoundException("Not enough product variant quantity");
            }

            return new OrderItem
            {
                VariantId = variant.Id,
                Name = variant.Title,
                Title = variant.Name ?? string.Empty,
                Price = variant.Price,
                Quantity = basketItem.Quantity,
                ImageUrl = variant.ImageUrl ?? string.Empty
            };
        }).ToList();

        var totalAmount = orderItems.Aggregate(
            Money.Zero(Currency.VND),
            (sum, item) => sum + Money.Vnd(item.TotalPrice));

        if (command.Method == PaymentMethod.COD && command.Provider != PaymentProvider.Unknown)
        {
            throw new Exception("COD does not use provider");
        }

        if (command.Method == PaymentMethod.Online && command.Provider == PaymentProvider.Unknown)
        {
            throw new Exception("Online payment requires provider");
        }

        var order = Domain.Entities.Order.Create(
            Guid.CreateVersion7(),
            DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            command.CustomerId,
            Address.Of(command.Street, command.City, command.ZipCode),
            orderItems,
            totalAmount,
            command.Method,
            command.Provider);

        await _orderRepository.AddAsync(order);
        await _orderRepository.UnitOfWork.SaveChangesAsync();

        string? paymentUrl = "";
        if (command.Method == PaymentMethod.Online)
        {
            var paymentSession = await _paymentService.CreatePaymentSessionAsync(new CreatePaymentSession
            {
                OrderNumber = order.OrderNumber,
                Amount = totalAmount.Amount,
                Provider = command.Provider
            }, cancellationToken);

            paymentUrl = paymentSession;
        }

        return new PlaceOrderDto
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber,
            PaymentUrl = paymentUrl
        };
    }
}

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.CustomerId)
         .NotEmpty().WithMessage("CustomerId is required.");

        RuleFor(x => x.Method)
         .IsInEnum().WithMessage("Invalid payment method.");

        RuleFor(x => x.Provider)
        .IsInEnum().WithMessage("PaymentProvider is invalid");
    }
}
