using EShop.Application.Variants.Services;
using EShop.Application.Variants.DTOs;
using MediatR;

namespace EShop.Application.Variants.Queries;

public record GetVariantByIdQuery(Guid VariantId) : IRequest<VariantDto>;

internal class GetVariantByIdQueryHandler : IRequestHandler<GetVariantByIdQuery, VariantDto>
{
    private readonly IVariantQueryService _variantQueryService;

    public GetVariantByIdQueryHandler(IVariantQueryService variantQueryService)
    {
        _variantQueryService = variantQueryService;
    }

    public async Task<VariantDto> Handle(GetVariantByIdQuery request, CancellationToken cancellationToken)
    {
        var variant = await _variantQueryService.GetVariantAsync(request.VariantId, cancellationToken);

        return variant;
    }
}