namespace EShop.Domain.Infrastructure.Storage;

public interface IFileStorageManager
{
    string GetFileUrl(IFileEntry fileEntry);
    Task CreateAsync(IFileEntry fileEntry, Stream stream, CancellationToken cancellationToken = default);
    Task DeleteAsync(IFileEntry fileEntry, CancellationToken cancellationToken = default);
}

public interface IFileEntry
{
    public string Id { get; set; }
    public string FileName { get; set; }
    public string FileLocation { get; set; }
}