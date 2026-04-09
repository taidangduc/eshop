using EShop.Application.FileEntries.Services;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Products.Commands;

public record DeleteProductOptionCommand(
    Guid ProductId,
    Guid OptionId) : IRequest<bool>;

internal class DeleteProductOptionCommandHandler : IRequestHandler<DeleteProductOptionCommand, bool>
{
    private readonly IProductRepository _productRepository;
    public DeleteProductOptionCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<bool> Handle(DeleteProductOptionCommand request, CancellationToken cancellationToken)
    {
        var query = _productRepository.GetQueryableSet()
            .Include(p => p.Options)
            .Where(p => p.Id == request.ProductId);

        var product = await _productRepository.FirstOrDefaultAsync(query);

        if (product is null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }

        product.RemoveOption(request.OptionId);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class DeleteProductOptionCommandValidator : AbstractValidator<DeleteProductOptionCommand>
{
    public DeleteProductOptionCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.OptionId)
            .NotEmpty().WithMessage("OptionId is required.");
    }
}