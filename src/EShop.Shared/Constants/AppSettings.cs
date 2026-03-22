namespace EShop.Shared.Constants;

public class AppSettings
{
    public Identity Identity { get; set; } = null!;
    public FileStorageSettings FileStorageSettings { get; set; } = null!;
    public ConnectionStrings ConnectionStrings { get; set; } = null!;
    public MailConfig MailConfig { get; set; } = null!;
    public VnpayConf VnpayConf { get; set; } = null!;
    public string BaseURL { get; set; } = "";
}
public class ConnectionStrings
{
    public string? DefaultConnection { get; set; }
}
public class FileStorageSettings
{
    public bool LocalStorage { get; set; } = true;
    public string Path { get; set; } = "";
}
public class MailConfig 
{
    public string From { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Host { get; set; } = null!;
    public int Port { get; set; }
}
public class Identity
{   
    public string Key { get; set; } = null!;
    public string Authority { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public int ExpiredTime { get; set; }
}

public class VnpayConf
{
    public string Version { get; set; } = "2.1.0";
    public string Command { get; set; } = "pay";
    public string TmnCode { get; set; } = null!;
    public string HashSecret { get; set; } = null!;
    public string CurrCode { get; set; } = "VND";
    public string Locale { get; set; } = "vn";
    public string OrderType { get; set; } = "other";
    public string ReturnUrl { get; set; } = null!;
    public string BaseUrl { get; set; } = null!;
}

public class BackgroundTaskOptions
{
    public int GracePeriodTime { get; set; } = 1;
    public int CheckUpdateTime { get; set; } = 30;
}
