using FluentValidation;

namespace EShop.Application.Customer.Commands;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull().WithMessage("UserId is required");
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");   
    }
}
