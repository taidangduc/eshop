using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
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
        _container = new BlobContainerClient(_options.ConnectionString, _options.Container);
    }

    private string GetBlobName(IFileEntry fileEntry)
    {
        return Path.Combine(_options.Path, fileEntry.FileLocation);
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

    public async Task CreateAsync(IFileEntry fileEntry, Stream stream, string? contentType = null, CancellationToken cancellationToken = default)
    {
        await _container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);

        BlobClient blob = _container.GetBlobClient(GetBlobName(fileEntry));

        var options = new BlobUploadOptions();

        if (!string.IsNullOrEmpty(contentType))
        {
            options.HttpHeaders = new BlobHttpHeaders
            {
                ContentType = contentType
            };
        }

        await blob.UploadAsync(stream, options, cancellationToken: cancellationToken);
    }

    public Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default)
    {
        BlobClient blobClient = _container.GetBlobClient(GetBlobName(fileEntry));
        return blobClient.DeleteAsync(cancellationToken: cancellationToken);
    }
}