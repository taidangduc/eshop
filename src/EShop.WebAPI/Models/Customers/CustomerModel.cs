namespace EShop.Api.Models.Customers;

public class CustomerModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
}