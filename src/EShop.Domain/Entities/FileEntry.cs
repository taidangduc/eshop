using EShop.Domain.Infrastructure.Storage;
using EShop.Domain.SeedWork;

namespace EShop.Domain.Entities;

public class FileEntry : Entity<Guid>, IAggregateRoot, IFileEntry
{
    public string FileName { get; set; }
    public string FileLocation { get; set; }
    public string ContentType { get; set; } 
}