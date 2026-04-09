using EShop.Domain.Entities;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EShop.Application.Products.Commands;

public record UpdateProductCommand(
    Guid ProductId,
    string? Title = null,
    string? Description = null,
    Guid? CategoryId = null) : IRequest<bool>;

internal class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IRepository<Product, Guid> _productRepository;

    public UpdateProductCommandHandler(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var query = _productRepository.GetQueryableSet()
            .Where(p => p.Id == request.ProductId);

        var product = await _productRepository.FirstOrDefaultAsync(query);

        if (product is null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }

        product.Update(request.Title, request.Description, request.CategoryId);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.Title)
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.")
            .When(x => x.Title is not null);

        RuleFor(x => x.Description)
            .MaximumLength(3000).WithMessage("Description must not exceed 3000 characters.")
            .When(x => x.Description is not null);
    }
}