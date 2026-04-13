using EShop.Contracts.Customer.Services;
using EShop.Contracts.Customer.DTOs;
using EShop.Domain.Repositories;

namespace EShop.Application.Customers.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task CreateAsync(CreateCustomerModel request)
    {
        var customer = new Domain.Entities.Customer
        {
            UserId = request.UserId,
            Email = request.Email
        };

        await _customerRepository.AddAsync(customer);
        await _customerRepository.UnitOfWork.SaveChangesAsync();
    }

    public async Task<CustomerDto> GetAsync(Guid UserId)
    {
        var customer = await _customerRepository.FirstOrDefaultAsync(_customerRepository.GetQueryableSet().Where(x => x.UserId == UserId));

        return customer != null ? MapToCustomerDto(customer) : new();
    }

    private CustomerDto MapToCustomerDto(Domain.Entities.Customer customer)
    {
        return new CustomerDto
        {
            Id = customer.Id,
            FullName = customer.FullName,
            Email = customer.Email,
            PhoneNumber = customer.PhoneNumber,
            Address = customer.Address
        };
    }
}