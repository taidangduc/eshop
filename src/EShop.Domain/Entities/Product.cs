using EShop.Domain.Enums;
using EShop.Domain.Events;
using EShop.Domain.SeedWork;

namespace EShop.Domain.Entities;

public class Product : AuditableEntity<Guid>, IAggregateRoot, ISoftDelete
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Guid CategoryId { get; set; }
    public ProductStatus Status { get; set; }
    public bool IsDeleted { get; set; }
    private readonly List<ProductImage> _images = new();
    private readonly List<ProductOption> _options = new();
    public IReadOnlyCollection<ProductImage> Images => _images.AsReadOnly();
    public IReadOnlyCollection<ProductOption> Options => _options.AsReadOnly();

    public void AddOption(string name, bool hasImage)
    {
        if (_options.Any(o => o.HasImage) && hasImage)
        {
            throw new InvalidOperationException("Only one product option can allow images per product.");
        }

        _options.Add(new ProductOption
        {
            Name = name,
            HasImage = hasImage
        });
    }

    public void RemoveOption(Guid optionId)
    {
        var option = _options.FirstOrDefault(o => o.Id == optionId);
        if (option is null)
        {
            throw new InvalidOperationException($"Option with ID {optionId} not found.");
        }

        var imageList = option.Values.Where(v => v.ImageUrl is not null).Select(v => v.ImageUrl).ToList();

        _options.Remove(option);

        foreach (var imageUrl in imageList)
        {
            AddDomainEvent(new FileEntryDeletedDomainEvent(imageUrl));
        }
    }

    public void AddOptionValue(Guid optionId, string value, string? fileLocation = null)
    {
        var option = _options.FirstOrDefault(o => o.Id == optionId);
        if (option is null)
        {
            throw new InvalidOperationException($"Option with ID {optionId} not found.");
        }

        if (!option.HasImage && fileLocation is not null)
        {
            throw new InvalidOperationException("Product option does not allow images.");
        }

        option.AddValue(value, fileLocation);
    }

    public void RemoveOptionValue(Guid optionId, Guid optionValueId)
    {
        var option = _options.FirstOrDefault(o => o.Id == optionId);
        if (option is null)
        {
            throw new InvalidOperationException($"Option with ID {optionId} not found.");
        }
        var value = option.Values.FirstOrDefault(v => v.Id == optionValueId);
        if (value is null)
        {
            throw new InvalidOperationException($"Option value with ID {optionValueId} not found.");
        }

        option.RemoveValue(optionValueId);

        if (option.HasImage && value.ImageUrl is not null)
        {
            AddDomainEvent(new FileEntryDeletedDomainEvent(value.ImageUrl));
        }

    }

    public void AddImage(bool isMain, int sortOrder, string fileLocation)
    {
        if (isMain && _images.Any(i => i.IsMain))
        {
            throw new InvalidOperationException("Only one main image is allowed per product.");
        }

        _images.Add(new ProductImage
        {
            IsMain = isMain,
            SortOrder = sortOrder,
            ImageUrl = fileLocation
        });
    }

    public void RemoveImage(Guid imageId)
    {
        var image = _images.FirstOrDefault(i => i.Id == imageId);
        if (image is null)
        {
            throw new InvalidOperationException($"Image with ID {imageId} not found.");
        }
        _images.Remove(image);

        AddDomainEvent(new FileEntryDeletedDomainEvent(image.ImageUrl));
    }

    public static Product Create(string title, string description, Guid categoryId)
    {
        var product = new Product
        {
            Id = Guid.CreateVersion7(),
            Title = title,
            Description = description,
            CategoryId = categoryId,
            Status = ProductStatus.Published,
        };

        return product;
    }

    public void Update(string? title = null, string? description = null, Guid? categoryId = null)
    {
        if (!string.IsNullOrWhiteSpace(title))
        {
            Title = title;
        }

        if (!string.IsNullOrWhiteSpace(description))
        {
            Description = description;
        }

        if (categoryId.HasValue)
        {
            CategoryId = categoryId.Value;
        }
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}