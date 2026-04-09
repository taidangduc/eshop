namespace EShop.EventBus;

public class MetaData
{
    public string MessageId { get; set; }
    public string CorrelationId { get; set; }
    public DateTime? CreationDateTime { get; set; }
    public DateTime? EnqueuedDateTime { get; set; }
}