using EShop.Domain.Infrastructure.Storage;

namespace EShop.Infrastructure.Storage;

public class LocalFileStorageManager : IFileStorageManager
{
    private readonly LocalOptions _options;

    public LocalFileStorageManager(LocalOptions options)
    {
        _options = options;
    }

    public string GetFileUrl(IFileEntry fileEntry)
    {
        var filePath = Path.Combine(_options.Path, fileEntry.FileLocation);
        return filePath;
    }

    public async Task CreateAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(_options.Path, fileEntry.FileLocation);

        var folder = Path.GetDirectoryName(filePath);

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await stream.CopyToAsync(fileStream, cancellationToken);
        }
    }

    public async Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        await Task.Run(() =>
        {
            var path = Path.Combine(_options.Path, fileEntry.FileLocation);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }, cancellationToken);
    }
}