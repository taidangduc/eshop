using EShop.Domain.Entities;
using EShop.Domain.Exceptions;
using EShop.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace EShop.Application.Variants.Commands;

public record DeleteVariantCommand(Guid VariantId) : IRequest<bool>;

internal class DeleteVariantCommandHandler(
    IRepository<Variant, Guid> variantRepository) : IRequestHandler<DeleteVariantCommand, bool>
{
    public async Task<bool> Handle(DeleteVariantCommand request, CancellationToken cancellationToken)
    {
        var query = variantRepository.GetQueryableSet()
            .Where(v => v.Id == request.VariantId);

        var variant = await variantRepository.FirstOrDefaultAsync(query);

        if (variant is null)
        {
            throw new NotFoundException($"Variant with ID {request.VariantId} not found.");
        }

        variant.Delete();

        return await variantRepository.UnitOfWork.SaveEntitiesAsync(cancellationToken);
    }
}

public class DeleteVariantCommandValidator : AbstractValidator<DeleteVariantCommand>
{
    public DeleteVariantCommandValidator()
    {
        RuleFor(x => x.VariantId)
            .NotEmpty().WithMessage("VariantId is required.");
    }
}