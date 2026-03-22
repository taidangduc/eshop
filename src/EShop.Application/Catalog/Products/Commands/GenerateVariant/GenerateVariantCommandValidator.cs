using FluentValidation;

namespace EShop.Application.Catalog.Products.Commands.GenerateVariant;

public class GenerateVariantCommandValidator : AbstractValidator<GenerateVariantCommand>
{
    public GenerateVariantCommandValidator()
    {
        RuleFor(x => x.OptionValueMap)
            .NotNull().WithMessage("Option values are required.")
            .Must(ov => ov != null && ov.Count > 0)
            .WithMessage("At least one option with values must be provided.");        
    }
}
