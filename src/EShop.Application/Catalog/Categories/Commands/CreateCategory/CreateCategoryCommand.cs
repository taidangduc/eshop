using MediatR;

namespace EShop.Application.Catalog.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(string Title, string UrlSlug) : IRequest<Guid>;