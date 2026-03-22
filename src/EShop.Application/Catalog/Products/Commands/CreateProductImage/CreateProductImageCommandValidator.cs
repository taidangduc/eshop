using FluentValidation;

namespace EShop.Application.Catalog.Products.Commands.CreateProductImage;

public class CreateProductImageCommandValidator : AbstractValidator<CreateProductImageCommand>
{
    public CreateProductImageCommandValidator()
    {
        RuleFor(x => x.ProductId).NotNull().WithMessage("ProductId is requried.");

        RuleFor(x => x.MediaFile).NotNull().WithMessage("MediaFile is requried.");    
    }
}
