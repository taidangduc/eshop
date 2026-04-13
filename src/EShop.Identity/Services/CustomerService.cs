using EShop.Contracts.Customer.DTOs;
using EShop.Contracts.Customer.Services;

namespace EShop.IdentityService.Services;

public class CustomerService : ICustomerService
{
    private readonly HttpClient _httpClient;
    public CustomerService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task CreateAsync(CreateCustomerModel customer)
    {
        return _httpClient.PostAsJsonAsync("api/v1/customers", customer);
    }

    public async Task<CustomerDto> GetAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<CustomerDto>($"api/v1/customers/{id}")
            ?? throw new Exception("Customer not found");
    }
}