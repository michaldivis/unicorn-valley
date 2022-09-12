using System.ComponentModel.DataAnnotations.Schema;

namespace UnicornValley.Domain.Common;
public abstract class EntityBase
{
    public Guid Id { get; }

    protected EntityBase(Guid id)
    {
        Id = id;
    }

    private readonly List<EventBase> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<EventBase> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(EventBase domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(EventBase domainEvent)
    {
        _ = _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
