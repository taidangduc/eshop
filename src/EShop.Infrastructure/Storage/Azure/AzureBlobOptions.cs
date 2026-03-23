namespace EShop.Infrastructure.Storage;

public class AzureBlobOptions
{
    public string ConnectionString { get; set; }
    public string ContainerName { get; set; }
    public string Path { get; set; }
}