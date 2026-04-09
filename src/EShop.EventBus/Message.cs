using System.Text;
using System.Text.Json;

namespace EShop.EventBus;

public class Message<T>
{
    public MetaData metaData { get; set; }
    public T message { get; set; }

    public string SerializeObject()
    {
        return JsonSerializer.Serialize(this);
    }

    public byte[] GetBytes()
    {
        return Encoding.UTF8.GetBytes(SerializeObject());
    }
}