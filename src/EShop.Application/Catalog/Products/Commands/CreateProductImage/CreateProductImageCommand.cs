using MediatR;
using Microsoft.AspNetCore.Http;

namespace EShop.Application.Catalog.Products.Commands.CreateProductImage;

public record CreateProductImageCommand(Guid ProductId, bool IsMain = false, IFormFile? MediaFile = null) : IRequest<Unit>;
//{
//    public IFormFile MediaFile { get; init; }
//}
