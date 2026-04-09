using EShop.Application.Products.DTOs;
using EShop.Application.Products.Services;
using MediatR;

namespace EShop.Application.Products.Queries;
        
public record GetProductQuery(Guid ProductId) : IRequest<ProductDetail>;

internal class GetProductQueryHandler : IRequestHandler<GetProductQuery, ProductDetail>
{
    private readonly IProductQueryService _productQueryService;

    public GetProductQueryHandler(IProductQueryService productQueryService)
    {
        _productQueryService = productQueryService;
    }

    public async Task<ProductDetail> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        var product = await _productQueryService.GetProductAsync(request.ProductId, cancellationToken);

        return product;
    }
}

