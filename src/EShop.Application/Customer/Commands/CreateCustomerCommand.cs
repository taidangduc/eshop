using MediatR;

namespace EShop.Application.Customer.Commands;

public record CreateCustomerCommand(Guid UserId, string Email) : IRequest<Guid>;