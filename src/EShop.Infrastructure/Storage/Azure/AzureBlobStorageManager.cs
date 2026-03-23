using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using EShop.Domain.Infrastructure.Storage;

namespace EShop.Infrastructure.Storage;

public class AzureBlobStorageManager : IFileStorageManager
{
    private readonly AzureBlobOptions _options;
    private readonly BlobContainerClient _container;

    public AzureBlobStorageManager(AzureBlobOptions options)
    {
        _options = options;
        _container = new BlobContainerClient(_options.ConnectionString, _options.ContainerName);
    }

    private string GetBlobName(IFileEntry fileEntry)
    {
        return _options.Path + fileEntry.FileLocation;
    }

    public string GetFileUrl(IFileEntry fileEntry)
    {
        BlobClient blobClient = _container.GetBlobClient(GetBlobName(fileEntry));
        var url = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTime.UtcNow.AddMinutes(30));
        return url.ToString();
    }

    public async Task CreateAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default)
    {
        await _container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        BlobClient blobClient = _container.GetBlobClient(GetBlobName(fileEntry));
        await blobClient.UploadAsync(stream, overwrite: true, cancellationToken);
    }

    public Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        BlobClient blobClient = _container.GetBlobClient(GetBlobName(fileEntry));
        return blobClient.DeleteAsync(cancellationToken: cancellationToken);
    }
}