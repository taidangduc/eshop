using AutoMapper;
using EShop.Domain.Entities;

namespace EShop.Application.Catalog.Categories.Dtos;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UrlSlug { get; set; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Category, CategoryDto>();
        }
    }
}

