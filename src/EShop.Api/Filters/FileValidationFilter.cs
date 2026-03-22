using FluentValidation;

namespace EShop.Api.Filters;

//ref: https://stackoverflow.com/questions/59358252/how-to-validate-uploaded-files-by-fluentvalidation
public class FileValidationFilter : IEndpointFilter
{
    private readonly long _maxFileSizeInBytes = 1048576; // 1 MB
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.HttpContext.Request;
        var formCollection = await request.ReadFormAsync();
        var files = formCollection.Files;

        if (files.Count == 0)
        {
            return await next(context);
        }

        foreach (var file in files)
        {
            if(file.Length == 0)
            {
                throw new ValidationException("File is empty.");
            }

            if (file.Length > _maxFileSizeInBytes)
            {
                throw new ValidationException($"File size is too large. Max file size is {_maxFileSizeInBytes / 1024} KB.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();

            if (allowedExtensions.Contains(fileExtension))
            {
                continue;
            }
            else
            {
                throw new ValidationException($"File type is not allowed. Allowed: {string.Join(", ", allowedExtensions)}");
            }
        }
        return await next(context);
    }
}
