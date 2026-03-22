namespace EShop.Api.Models.Requests;

public class PaginationRequest(int pageSize = 10, int pageIndex = 1)
{
    public int PageIndex { get; set; } = pageIndex;
    public int PageSize { get; set; } = pageSize;
}
