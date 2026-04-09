using EShop.Application.FileEntries.Services;
using EShop.Contracts.IntegrationEvents;
using EShop.EventBus;

namespace EShop.Infrastructure.IntegrationEventHandlers;

public class FileEntryConsumer : IIntegrationEventHandler<FileEntryDeletedEvent>
{
    private readonly IFileEntryService _fileEntryService;

    public FileEntryConsumer(IFileEntryService fileEntryService)
    {
        _fileEntryService = fileEntryService;
    }

    public Task HandleAsync(FileEntryDeletedEvent message, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrEmpty(message.FileLocation))
        {
            return _fileEntryService.DeleteFileEntryAsync(message.FileLocation, cancellationToken);
        }

        return Task.CompletedTask;
    }
}