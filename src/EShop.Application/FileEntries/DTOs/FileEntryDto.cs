using EShop.Domain.Infrastructure.Storage;

namespace EShop.Application.FileEntries.DTOs;

public class FileEntryDto : IFileEntry
{
    public string FileLocation { get; set; }
}