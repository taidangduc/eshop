using EShop.Application.FileEntries.Services;
using EShop.Domain.Events;
using MediatR;

namespace EShop.Application.Products;

public class FileEntryDeletedDomainHandler : INotificationHandler<FileEntryDeletedDomainEvent>
{
    private readonly IFileEntryService _fileEntryService;

    public FileEntryDeletedDomainHandler(IFileEntryService fileEntryService)
    {
        _fileEntryService = fileEntryService;
    }

    public Task Handle(FileEntryDeletedDomainEvent request, CancellationToken cancellationToken)
    {
        if(!string.IsNullOrEmpty(request.FileLocation))
        {
            return _fileEntryService.DeleteFileEntryAsync(request.FileLocation);
        }
        return Task.CompletedTask;
    }
}