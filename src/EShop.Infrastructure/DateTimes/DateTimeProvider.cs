namespace EShop.Infrastructure.DateTimes;

public class DateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTimeOffset OffsetNow => DateTimeOffset.Now;
    public DateTimeOffset OffsetUtcNow => DateTimeOffset.UtcNow;
}