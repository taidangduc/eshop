namespace EShop.Api.Models.Products;

public class CreateProductImageModel
{
    public Guid ProductId { get; set; }
    public bool IsMain { get; set; }
    
    public IFormFile FormFile { get; set; }
}
