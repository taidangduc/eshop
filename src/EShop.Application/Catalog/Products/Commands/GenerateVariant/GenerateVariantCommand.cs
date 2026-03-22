using MediatR;

namespace EShop.Application.Catalog.Products.Commands.GenerateVariant;
public record GenerateVariantCommand(Guid ProductId, Dictionary<Guid, List<Guid>>? OptionValueMap) : IRequest<Unit>;
