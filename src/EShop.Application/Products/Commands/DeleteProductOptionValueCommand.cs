using EShop.Application.FileEntries.Services;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EShop.Application.Products.Commands;

public record DeleteProductOptionValueCommand(
    Guid ProductId,
    Guid OptionId,
    Guid OptionValueId) : IRequest<bool>;

internal class DeleteProductOptionValueCommandHandler : IRequestHandler<DeleteProductOptionValueCommand, bool>
{
    private readonly IProductRepository _productRepository;
    public DeleteProductOptionValueCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(DeleteProductOptionValueCommand request, CancellationToken cancellationToken)
    {
        var query = _productRepository.GetQueryableSet()
            .Include(p => p.Options)
                .ThenInclude(o => o.Values)
            .Where(p => p.Id == request.ProductId);

        var product = await _productRepository.FirstOrDefaultAsync(query);

        if (product is null)
        {
            throw new NotFoundException($"Product with ID {request.ProductId} not found.");
        }

        product.RemoveOptionValue(request.OptionId, request.OptionValueId);

        await _productRepository.UpdateAsync(product, cancellationToken);   

        return await _productRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class DeleteProductOptionValueCommandValidator : AbstractValidator<DeleteProductOptionValueCommand>
{
    public DeleteProductOptionValueCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.OptionId)
            .NotEmpty().WithMessage("OptionId is required.");

        RuleFor(x => x.OptionValueId)
            .NotEmpty().WithMessage("OptionValueId is required.");
    }
}