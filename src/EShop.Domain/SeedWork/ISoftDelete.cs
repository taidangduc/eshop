namespace EShop.Domain.SeedWork;

public interface ISoftDelete
{
    bool IsDeleted { get; set; }
    void Delete();
}