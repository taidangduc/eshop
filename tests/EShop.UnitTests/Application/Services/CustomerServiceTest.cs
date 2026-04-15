using EShop.Application.Customers.Services;
using EShop.Contracts.Customer.DTOs;
using EShop.Contracts.Customer.Services;
using EShop.Domain.Entities;
using EShop.Domain.Repositories;
using Moq;

namespace EShop.UnitTests.Application.Customers;

public class CustomerServiceTest
{
    private readonly Mock<ICustomerRepository> _customerRepository;
    private readonly Mock<IUnitOfWork> _unitOfWork;
    private readonly ICustomerService _customerService;

    public CustomerServiceTest()
    {
        _customerRepository = new Mock<ICustomerRepository>();
        _unitOfWork = new Mock<IUnitOfWork>();
        _customerRepository.Setup(x => x.UnitOfWork).Returns(() => _unitOfWork.Object);
        _customerService = new CustomerService(_customerRepository.Object);
    }

    [Fact]
    public async Task CreateCustomer_WhenUserCreated_ReturnSuccess()
    {
        // Arrange
        var userId = Guid.CreateVersion7();
        var email = "test@example.com";

        _customerRepository
            .Setup(r => r.AddAsync(It.IsAny<Customer>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWork
            .Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        await _customerService.CreateAsync(new CreateCustomerModel { UserId = userId, Email = email });

        // Assert
        _customerRepository.Verify(x => 
            x.AddAsync(
                It.Is<Customer>(e => e.UserId == userId && e.Email == email),
                It.IsAny<CancellationToken>()),
            Times.Once);

        _unitOfWork.Verify(x => 
            x.SaveChangesAsync(It.IsAny<CancellationToken>()), 
            Times.Once);
    }
}