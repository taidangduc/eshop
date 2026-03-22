using FluentValidation;

namespace EShop.Application.Catalog.Products.Queries.GetVariantByOption;

public class GetVariantByOptionQueryValidator : AbstractValidator<GetVariantByOptionQuery>
{
    public GetVariantByOptionQueryValidator()
    {
        RuleFor(x => x.ProductId)
            .NotNull().WithMessage("ProductId must be greater than 0.");
        RuleFor(x => x.OptionValueMap)
            .NotNull().WithMessage("OptionFilter cannot be null.")
            .Must(of => of != null && of.Count > 0).WithMessage("OptionFilter must contain at least one option.");
    }
}
