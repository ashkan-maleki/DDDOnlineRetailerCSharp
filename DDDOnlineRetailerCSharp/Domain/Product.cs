namespace DDDOnlineRetailerCSharp.Domain;

public class Product(string sku, ICollection<Batch> batches, int versionNumber = 0)
{
    private Product(string sku, int versionNumber = 0) : this(sku, new List<Batch>(), versionNumber)
    {
    }

    public string Sku { get; } = sku;

    public int VersionNumber { get; private set; } = versionNumber;

    public ICollection<Batch> Batches { get; } = batches;

    // public List<Event> Events { get; init; } = new();
    private readonly HashSet<Event> _events = new();

    public void AddEvent(Event @event) => _events.Add(@event);

    public bool HasEvent() => _events.Any();

    public bool HasOutOfStockEvent(string sku)
    {
        if (_events.LastOrDefault() is OutOfStock outOfStock)
        {
            return outOfStock.Sku == sku;
        }

        return false;
    }

    public Event? PopEvent()
    {
        Event? @event = _events.FirstOrDefault();
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
            _events!.Add(new OutOfStock(line.Sku));
            return null;
        }

        batch!.Allocate(line);
        VersionNumber++;
        return batch.Reference!;
    }
}