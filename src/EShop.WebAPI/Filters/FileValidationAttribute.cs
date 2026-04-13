using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EShop.Api.Filters;

//ref: https://stackoverflow.com/questions/59358252/how-to-validate-uploaded-files-by-fluentvalidation
public class FileValidationAttribute : ActionFilterAttribute
{
    private readonly long _maxFileSizeInBytes = 10485760; // 10 MB

    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var request = context.HttpContext.Request;
        var formCollection = await request.ReadFormAsync();
        var files = formCollection.Files;

        if (files.Count == 0)
        {
            await next();
            return;
        }

        foreach (var file in files)
        {
            if(file.Length == 0)
            {
                throw new ValidationException("File is empty.");
            }

            if (file.Length > _maxFileSizeInBytes)
            {
                throw new ValidationException($"File size is too large. Max file size is 10 MB.");
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var fileExtension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new ValidationException($"File type is not allowed. Allowed: {string.Join(", ", allowedExtensions)}");
            }
        }
        await next();
    }
}
