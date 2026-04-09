using EShop.Application.Categories.DTOs;
using EShop.Domain.Entities;
using EShop.Domain.Repositories;
using MediatR;

namespace EShop.Application.Categories.Queries;

public record GetCategoriesQuery : IRequest<List<CategoryDto>>;

internal class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly IRepository<Category, Guid> _repository;

    public GetCategoriesQueryHandler(IRepository<Category, Guid> repository)
    {
        _repository = repository;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.ToListAsync(_repository.GetQueryableSet());

        return DomainToDtoMapper.ToCategoryDtos(categories);
    }
}