namespace EShop.Application.Abstractions;

public interface ICurrentUserProvider
{
    string? GetCurrentUserId();
}
