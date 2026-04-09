namespace EShop.Infrastructure.Storage;

public class AzureBlobOptions
{
    public string ConnectionString { get; set; }
    public string Container { get; set; }
    public string Path { get; set; }
}