
namespace EShop.Api.Models.Requests;

public record CreateProductImageRequestDto(Guid ProductId, bool IsMain = false, IFormFile? MediaFile = null);
