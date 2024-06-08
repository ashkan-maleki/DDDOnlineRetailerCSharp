using System.Text.Json.Serialization;

namespace DDDOnlineRetailerCSharp.EventBus;

public record Event
{
    [JsonInclude]
    public Guid Id { get; set; } = Guid.NewGuid();

    [JsonInclude]
    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
}