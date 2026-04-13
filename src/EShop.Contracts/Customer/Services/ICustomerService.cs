using EShop.Contracts.Customer.DTOs;

namespace EShop.Contracts.Customer.Services;

public interface ICustomerService
{
    Task CreateAsync(CreateCustomerModel customer);
    Task<CustomerDto> GetAsync(Guid id);
}