using EShop.Application.FileEntries.DTOs;
using EShop.Domain.Infrastructure.Storage;
using Microsoft.AspNetCore.Http;

namespace EShop.Application.FileEntries.Services;

public class FileEntryService : IFileEntryService
{
    private readonly IFileStorageManager _fileStorageManager;
    public FileEntryService(IFileStorageManager fileStorageManager)
    {
        _fileStorageManager = fileStorageManager;
    }
    // In real, 
    // you should save the file entry info to database and return the file entry id, then you can get the file url by file entry id.
    // But for simplicity, we just return the file location here.

    public async Task<FileEntryDto> CreateFileEntryAsync(IFormFile fromfile, CancellationToken cancellationToken = default)
    {
        var fileEntry = new FileEntryDto
        {
            FileLocation = $"images/{DateTime.Now.ToString("yyyy/MM/dd/") + Guid.NewGuid()}"
        };

        await _fileStorageManager.CreateAsync(fileEntry, fromfile.OpenReadStream(), fromfile.ContentType, cancellationToken);
        return fileEntry;
    }

    public async Task DeleteFileEntryAsync(string fileLocation, CancellationToken cancellationToken = default)
    {
        var fileEntry = new FileEntryDto { FileLocation = fileLocation };
        await _fileStorageManager.DeleteAsync(fileEntry, cancellationToken);
    }

    public string? GetFileUrlAsync(string fileLocation, CancellationToken cancellationToken = default)
    {
        var fileEntry = new FileEntryDto { FileLocation = fileLocation };
        var result = _fileStorageManager.GetFileUrl(fileEntry);
        return result;
    }
}