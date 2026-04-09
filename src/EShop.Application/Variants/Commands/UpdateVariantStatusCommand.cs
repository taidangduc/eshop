using EShop.Domain.Entities;
using EShop.Domain.Enums;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using MediatR;

namespace EShop.Application.Variants.Commands;

public record UpdateVariantStatusCommand(
    Guid VariantId,
    VariantStatus Status) : IRequest<bool>;

internal class UpdateVariantStatusCommandHandler : IRequestHandler<UpdateVariantStatusCommand, bool>
{
    private readonly IRepository<Variant, Guid> _variantRepository;

    public UpdateVariantStatusCommandHandler(IRepository<Variant, Guid> variantRepository)
    {
        _variantRepository = variantRepository;
    }


    public async Task<bool> Handle(UpdateVariantStatusCommand request, CancellationToken cancellationToken)
    {
        var query = _variantRepository.GetQueryableSet()
            .Where(v => v.Id == request.VariantId);

        var variant = await _variantRepository.FirstOrDefaultAsync(query);

        if (variant is null)
        {
            throw new NotFoundException($"Variant with ID {request.VariantId} not found.");
        }

        variant.Status = request.Status;

        await _variantRepository.UpdateAsync(variant, cancellationToken);

        return await _variantRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}