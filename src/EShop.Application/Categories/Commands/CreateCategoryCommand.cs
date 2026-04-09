using EShop.Domain.Entities;
using EShop.Domain.Repositories;
using MediatR;

namespace EShop.Application.Categories.Commands;

public record CreateCategoryCommand(string Name) : IRequest<Guid>;

internal class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly IRepository<Category, Guid> _repository;

    public CreateCategoryCommandHandler(IRepository<Category, Guid> repository)
    {
        _repository = repository;
    }
    
    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name
        };

        await _repository.AddAsync(category);
        await _repository.UnitOfWork.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}