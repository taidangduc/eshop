using EShop.Application.Order.Dtos;
using EShop.Domain.Enums;
using MediatR;

namespace EShop.Application.Order.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid CustomerId,
    PaymentMethod Method, 
    PaymentProvider Provider,
    string Street,
    string City, 
    string ZipCode) : IRequest<CreateOrderResult>;
