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

    public async Task CreateAsync(CreateCustomerModel customer)
    {
        var response = await _httpClient.PostAsJsonAsync("api/v1/customers", customer);
        response.EnsureSuccessStatusCode();
    }

    public async Task<CustomerDto> GetAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"api/v1/customers/{id}");
        if (response.StatusCode == System.Net.HttpStatusCode.NotFound) return new();
        response.EnsureSuccessStatusCode();
        var customer = await response.Content.ReadFromJsonAsync<CustomerDto>();
        return customer ?? throw new Exception("Customer not found");
    }
}