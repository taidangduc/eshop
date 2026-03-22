using EShop.Application.Catalog.Products.Services;
using EShop.Application.Common.Exceptions;
using EShop.Domain.Entities;
using EShop.Domain.Repositories;
using MediatR;

namespace EShop.Application.Catalog.Products.Commands.GenerateVariant;

public class GenerateVariantCommandHandler : IRequestHandler<GenerateVariantCommand, Unit>
{
    private readonly IProductRepository _productRepository;
    private readonly IProductVariantService _productVariantService;

    public GenerateVariantCommandHandler(
        IProductRepository productRepository,
        IProductVariantService productVariantService)
    {
        _productRepository = productRepository;
        _productVariantService = productVariantService;
    }

    public async Task<Unit> Handle(GenerateVariantCommand request, CancellationToken cancellationToken)
    {
        if (request.OptionValueMap == null || request.OptionValueMap.Count == 0)
            throw new ArgumentException("No option values provided.");

        var query = _productRepository.GetQueryableSet()
            .Where(p => p.Id == request.ProductId);

        var product = await _productRepository.SingleOrDefaultAsync(query)
            ?? throw new EntityNotFoundException(nameof(Product), request.ProductId);

        await _productVariantService.GenerateVariantsAsync(
            product,
            request.OptionValueMap,
            cancellationToken);

        await _productRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
    /* END NEW v2.0 */
}

