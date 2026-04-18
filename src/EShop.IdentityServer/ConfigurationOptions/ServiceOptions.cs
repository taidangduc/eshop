namespace EShop.IdentityService.ConfigurationOptions;

public class ServiceOptions
{
    public CustomerSeviceOptions Customer { get; set; }
}

public class CustomerSeviceOptions
{
    public string BaseUrl { get; set; }
}