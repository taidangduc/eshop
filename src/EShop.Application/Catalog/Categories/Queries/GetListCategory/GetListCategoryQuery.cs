using EShop.Application.Catalog.Categories.Dtos;
using AutoMapper;
using EShop.Domain.Entities;
using EShop.Domain.Repositories;
using MediatR;

namespace EShop.Application.Catalog.Categories.Queries.GetListCategory;

public record GetListCategoryQuery : IRequest<List<CategoryDto>>;

public class GetListCategoryQueryHandler : IRequestHandler<GetListCategoryQuery, List<CategoryDto>>
{
    private readonly IRepository<Category, Guid> _repository;
    private readonly IMapper _mapper;

    public GetListCategoryQueryHandler(
        IMapper mapper,
        IRepository<Category, Guid> repository)
    {
        _mapper = mapper;
        _repository = repository;
    }

    public async Task<List<CategoryDto>> Handle(GetListCategoryQuery request, CancellationToken cancellationToken)
    {
        var categories = await _repository.ToListAsync(_repository.GetQueryableSet());

        return _mapper.Map<List<CategoryDto>>(categories);
    }
}