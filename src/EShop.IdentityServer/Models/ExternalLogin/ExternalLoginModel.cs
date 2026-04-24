namespace EShop.IdentityService.Models.ExternalLogin;

public class ExternalLoginModel
{
    public string Provider { get; set; }
    public string? ReturnUrl { get; set; }
}
