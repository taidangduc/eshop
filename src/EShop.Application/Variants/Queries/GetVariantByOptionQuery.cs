using EShop.Application.Variants.Services;
using EShop.Application.Variants.DTOs;
using MediatR;

namespace EShop.Application.Variants.Queries;

public record GetVariantByOptionQuery(Guid ProductId, List<Guid> OptionValueIds) : IRequest<VariantOverview>;

internal class GetVariantByOptionQueryHandler : IRequestHandler<GetVariantByOptionQuery, VariantOverview>
{
    private readonly IVariantQueryService _variantQueryService;

    public GetVariantByOptionQueryHandler(IVariantQueryService variantQueryService)
    {
        _variantQueryService = variantQueryService;
    }

    public async Task<VariantOverview> Handle(GetVariantByOptionQuery request, CancellationToken cancellationToken)
    {
        var variant = await _variantQueryService.GetVariantByOptionAsync(request.ProductId, request.OptionValueIds, cancellationToken);

        return variant;
    }
}