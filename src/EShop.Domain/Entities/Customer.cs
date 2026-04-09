using EShop.Domain.SeedWork;

namespace EShop.Domain.Entities;

public class Customer : Entity<Guid>, IAggregateRoot
{
    public Guid UserId { get; set; }
    public string? FullName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; } 
    public string? Address { get; set; }
}