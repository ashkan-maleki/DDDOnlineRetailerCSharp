using System.Text.Json.Serialization;

namespace DDDOnlineRetailerCSharp.Domain;

public record Event
{
    [JsonInclude]
    public Guid Id { get; set; }
    
    [JsonInclude]
    public DateTime CreationDate { get; set; }

    public Event()
    {
        Id = Guid.NewGuid();
        CreationDate = DateTime.UtcNow;
    }
    
}

public record OutOfStock(string Sku) : Event
{
    public string Sku { get; } = Sku;
}