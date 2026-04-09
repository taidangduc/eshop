namespace EShop.Application.Customers.DTOs;

public class DomainMapToDtoMapper
{
    public static CustomerDto ToCustomerDTO(Domain.Entities.Customer customer)
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