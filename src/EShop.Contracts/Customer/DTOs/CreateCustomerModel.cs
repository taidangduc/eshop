namespace EShop.Contracts.Customer.DTOs;

public class CreateCustomerModel
{
    public Guid UserId { get; set; }
    public string Email { get; set; }
}