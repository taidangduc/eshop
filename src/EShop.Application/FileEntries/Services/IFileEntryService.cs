using EShop.Application.FileEntries.DTOs;
using Microsoft.AspNetCore.Http;

namespace EShop.Application.FileEntries.Services;

public interface IFileEntryService
{
    Task<FileEntryDto> CreateFileEntryAsync(IFormFile fromfile, CancellationToken cancellationToken = default);
    Task DeleteFileEntryAsync(string fileLocation, CancellationToken cancellationToken = default);
    string? GetFileUrlAsync(string fileLocation, CancellationToken cancellationToken = default);
}