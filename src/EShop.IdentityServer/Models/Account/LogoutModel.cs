namespace EShop.IdentityService.Models.Account;

public class LogoutModel
{
    public string LogoutId { get; set; }
    /// <summary>
    /// Just display confirm logout screen when user was IsAuthenticated()
    /// else if, slient logout, skip confirm logout step
    /// </summary>
    public bool ShowLogoutPrompt { get; set; } = true;
    /// <summary>
    /// bind for search param which is address return from root domain called 
    /// </summary>
    public string? PostLogoutRedirectUri { get; set; }
}
