using EShop.Application.Abstractions;

namespace EShop.Api.Services;

public class CurrentIPAddress : ICurrentIPAddress
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentIPAddress(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? GetCurrentIPAddress()
    {
        return _httpContextAccessor?.HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}
