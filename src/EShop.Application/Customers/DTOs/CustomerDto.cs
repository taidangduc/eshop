namespace EShop.Application.Customers.DTOs;

public class CustomerDto 
{
    public Guid Id { get; init; }
    public string? FullName { get; init; }
    public string? Email { get; init; }
    public string? PhoneNumber { get; init; }
    public string? Address { get; init; }
}

