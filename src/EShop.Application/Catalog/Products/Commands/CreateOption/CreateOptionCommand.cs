using MediatR;

namespace EShop.Application.Catalog.Products.Commands.CreateOption;

public record CreateOptionCommand(Guid ProductId, string OptionName, bool AllowImage = false) : IRequest<Unit>;