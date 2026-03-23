using EShop.Domain.Infrastructure.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure.Storage;

public static class StorageCollectionExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services, StorageOptions options)
    {
        if (options.UsedLocal())
        {
            services.AddSingleton<IFileStorageManager>(new LocalFileStorageManager(options.Local));
        }
        else if (options.UsedAzureBlob())
        {
            services.AddSingleton<IFileStorageManager>(new AzureBlobStorageManager(options.Azure));
        }
        
        return services;
    }
}