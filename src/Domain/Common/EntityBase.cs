using System.ComponentModel.DataAnnotations.Schema;

namespace UnicornValley.Domain.Common;
public abstract class EntityBase
{
    public Guid Id { get; }

    protected EntityBase(Guid id)
    {
        Id = id;
    }

    private readonly List<DomainEventBase> _domainEvents = new();

    [NotMapped]
    public IReadOnlyCollection<DomainEventBase> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEventBase domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEventBase domainEvent)
    {
        _ = _domainEvents.Remove(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
