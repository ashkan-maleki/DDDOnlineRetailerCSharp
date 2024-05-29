using System.Text.Json.Serialization;

namespace DDDOnlineRetailerCSharp.Domain;



public record Command
{
    [JsonInclude]
    public Guid Id { get; set; } = Guid.NewGuid();
}

public record CreateBatch(string Reference, string Sku, int Qty, DateTime? Eta = null) : Command;

public record ChangeBatchQuantity(string Reference, int Qty) : Command;

public record Allocate(string OrderId, string Sku, int Qty) : Command;