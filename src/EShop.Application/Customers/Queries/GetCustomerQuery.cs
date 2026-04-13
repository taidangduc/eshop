using EShop.Contracts.Customer.DTOs;
using EShop.Contracts.Customer.Services;
using MediatR;

namespace EShop.Application.Customers.Queries;

public record GetCustomerQuery(Guid UserId) : IRequest<CustomerDto>;

public class GetCustomerByIdQueryHandler : IRequestHandler<GetCustomerQuery, CustomerDto>
{
    private readonly ICustomerService _customerService;

    public GetCustomerByIdQueryHandler(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    public async Task<CustomerDto> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetAsync(request.UserId);

        return customer;
    }
}
