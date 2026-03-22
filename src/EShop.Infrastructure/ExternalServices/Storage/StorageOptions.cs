using EShop.Infrastructure.ExternalServices.Storage.Local;

namespace EShop.Infrastructure.ExternalServices.Storage;
public class StorageOptions
{
    public string Provider { get; set; }
    public string FolderPath { get; set; }
    public LocalOptions Local { get; set; }
}
