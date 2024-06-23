namespace DDDOnlineRetailerCSharp.Domain;

public class Product(string sku, ICollection<Batch> batches, int versionNumber = 0)
{
    private Product(string sku, int versionNumber = 0) : this(sku, new List<Batch>(), versionNumber)
    {
    }

    public string Sku { get; } = sku;

    public int VersionNumber { get; private set; } = versionNumber;

    public ICollection<Batch> Batches { get; } = batches;

    public HashSet<DomainEvent> Events() => _events;

    // public List<Event> Events { get; init; } = new();
    // TODO: Convert to Queue
    private readonly HashSet<DomainEvent> _events = new();

    public void AddEvent(DomainEvent @event) => _events.Add(@event);

    public bool HasEvent() => _events.Any();

    public bool HasOutOfStockEvent(string sku)
    {
        if (_events.LastOrDefault() is OutOfStockDomainEvent outOfStock)
        {
            return outOfStock.Sku == sku;
        }

        return false;
    }

    public DomainEvent? PopEvent()
    {
        DomainEvent? @event = _events.FirstOrDefault();
        if (@event == null)
        {
            return null;
        }

        _events.Remove(@event);
        return @event;
    }


    public string? Allocate(OrderLine line)
    {
        Batch? batch = Batches.Order().FirstOrDefault(b => b.CanAllocate(line));
        if (batch == null)
        {
            _events!.Add(new OutOfStockDomainEvent(line.Sku));
            return null;
        }

        batch!.Allocate(line);
        VersionNumber++;
        _events!.Add(new AllocatedDomainEvent(line.OrderId, line.Sku, line.Qty, batch.Reference!));
        return batch.Reference!;
    }

    public void ChangeBatchQuantity(string reference, int qty)
    {
        Batch? batch = Batches.FirstOrDefault(b => b.Reference == reference);
        if (batch != null)
        {
            batch.SetPurchasedQuantity(qty);
            while (batch.AvailableQuantity < 0)
            {
                OrderLine? line = batch.DeallocateOne();
                if (line == null)
                {
                    break;
                }
                _events.Add(new DeallocatedDomainEvent(line.OrderId, line.Sku, line.Qty, batch.Reference));
            }
        }
    }
}