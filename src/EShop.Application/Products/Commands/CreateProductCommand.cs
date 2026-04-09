using EShop.Domain.Entities;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EShop.Application.Products.Commands;

public record CreateProductCommand(
    string Title,
    string Description,
    Guid CategoryId) : IRequest<Guid>;

internal class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IRepository<Product, Guid> _productRepository;
    private readonly IRepository<Category, Guid> _categoryRepository;

    public CreateProductCommandHandler(IRepository<Product, Guid> productRepository, IRepository<Category, Guid> categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.FirstOrDefaultAsync(
            _categoryRepository.GetQueryableSet().Where(c => c.Id == request.CategoryId));

        if (category is null)
        {
            throw new NotFoundException($"Category with ID {request.CategoryId} not found.");
        }

        var product = Product.Create(request.Title, request.Description, request.CategoryId);

        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return product.Id;
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title must not exceed 255 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(3000).WithMessage("Description must not exceed 3000 characters.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");
    }
}