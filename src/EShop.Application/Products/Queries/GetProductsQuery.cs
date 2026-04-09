using EShop.Application.Products.DTOs;
using EShop.Application.Products.Services;
using MediatR;

namespace EShop.Application.Products.Queries;

public record GetProductsQuery() : IRequest<List<ProductOverview>>;

internal class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, List<ProductOverview>>
{
    private readonly IProductQueryService _productQueryService;

    public GetProductsQueryHandler(IProductQueryService productQueryService)
    {
        _productQueryService = productQueryService;
    }

    public async Task<List<ProductOverview>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productQueryService.GetProductsAsync(cancellationToken);

        return products;
    }
}