namespace EShop.Application.Categories.DTOs;

public static class DomainToDtoMapper
{
    public static List<CategoryDto> ToCategoryDtos(this List<Domain.Entities.Category> categories)
    {
        return categories.Select(category => new CategoryDto
        {
            Id = category.Id,
            Name = category.Name
        }).ToList();
    }
}