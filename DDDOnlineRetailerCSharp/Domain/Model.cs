namespace DDDOnlineRetailerCSharp.Domain;



public record OrderLine(string OrderId, string Sku, int Qty);

public class Batch(string? reference, string? sku, int purchasedQuantity, DateTime? eta = null) : IComparable<Batch>
{
    private string? _sku = sku;
    private DateTime? _eta = eta;
    private int _purchasedQuantity = purchasedQuantity;
    private List<OrderLine> _allocations = new();
    private readonly string? _reference = reference;
    
    
    public string? Reference => _reference;
    public string? Sku => _sku;
    public DateTime? Eta => _eta;
    public int PurchasedQuantity => _purchasedQuantity;
    public List<OrderLine> Allocations => _allocations;
    public int AllocatedQuantity => Allocations.Sum(ol => ol.Qty);
    public int AvailableQuantity => PurchasedQuantity - AllocatedQuantity;

    
    
    public bool CanAllocate(OrderLine line) => Sku == line.Sku && AvailableQuantity >= line.Qty;
    public void Allocate(OrderLine line)
    {
        if (CanAllocate(line))
        {
            _allocations.Add(line);
        }
    }
    public void Deallocate(OrderLine line)
    {
        if (_allocations.Contains(line))
        {
            _allocations.Remove(line);
        }
    }


    public override string ToString() => $"<Batch {Reference}>";
    public int CompareTo(Batch? other)
    {
        // A null value means that this object is smaller.
        if (Eta == null)
            return -1;
        
        // A null value means that this object is greater.
        if (other == null || other.Eta == null)
            return 1;

        return Eta.Value.CompareTo(other.Eta.Value);
    }

    public override bool Equals(object? obj)
    {
        if (obj is Batch batch) // Nullable types are not allowed in patterns
        {
            return batch.Reference == Reference;
        }

        return false;
    }
    public override int GetHashCode() => HashCode.Combine(Reference);
    
    public static bool operator ==(Batch? lhs, Batch? rhs)
    {
        if (lhs is null)
        {
            return rhs is null;
        }
        return lhs.Equals(rhs);
    }
    public static bool operator >(Batch lhs, Batch rhs)
    {
        if (lhs.Eta is null)
        {
            return false;
        }

        if (rhs.Eta is null)
        {
            return true;
        }

        return lhs.Eta > rhs.Eta;
    }
    public static bool operator <(Batch lhs, Batch rhs) => rhs > lhs;
    public static bool operator !=(Batch lhs, Batch rhs) => !(lhs == rhs);
}