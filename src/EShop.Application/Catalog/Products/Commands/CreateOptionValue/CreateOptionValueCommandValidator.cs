using FluentValidation;

namespace EShop.Application.Catalog.Products.Commands.CreateOptionValue;

public class CreateOptionValueCommandValidator : AbstractValidator<CreateOptionValueCommand>
{
    public CreateOptionValueCommandValidator()
    {
        RuleFor(x => x.OptionId).NotEmpty();
        RuleFor(x => x.Value).NotEmpty().WithMessage("Option value is required");

        RuleFor(x => x.MediaFile).NotNull().WithMessage("MediaFile is requried.");
    }
}
