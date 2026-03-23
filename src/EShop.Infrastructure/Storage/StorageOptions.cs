namespace EShop.Infrastructure.Storage;

public  class StorageOptions
{
    public string Provider { get; set; }
    public LocalOptions Local { get; set; }
    public AzureBlobOptions Azure { get; set; }

    public bool UsedLocal()
    {
        return Provider == "Local";
    }

    public bool UsedAzureBlob()
    {
        return Provider == "Azure";
    }
}