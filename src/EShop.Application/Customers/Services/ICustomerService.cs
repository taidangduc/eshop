using EShop.Application.Customers.DTOs;

namespace EShop.Application.Customers.Services;

public interface ICustomerService
{
    Task<CustomerDto> GetCustomerAsync(Guid UserId);
}