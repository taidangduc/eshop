using EShop.Application.Customers.DTOs;
using EShop.Domain.Repositories;

namespace EShop.Application.Customers.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task<CustomerDto> GetCustomerAsync(Guid UserId)
    {
        var customer = await _customerRepository.FirstOrDefaultAsync(_customerRepository.GetQueryableSet().Where(x => x.UserId == UserId));

        return customer != null ? DomainMapToDtoMapper.ToCustomerDTO(customer) : new();
    }
}