using EShop.Domain.Repositories;
using MediatR;

namespace EShop.Application.Customers.Commands;

public record CreateCustomerCommand(Guid UserId, string Email) : IRequest<Guid>;

internal class CreateCustomerCommandHandler : IRequestHandler<CreateCustomerCommand, Guid>
{
    private readonly ICustomerRepository _repository;

    public CreateCustomerCommandHandler(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = new Domain.Entities.Customer
        {
            Id = Guid.CreateVersion7(),
            UserId = request.UserId,
            Email = request.Email
        };

        await _repository.AddAsync(customer);

        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return customer.Id;
    }
}