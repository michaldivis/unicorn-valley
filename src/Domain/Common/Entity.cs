namespace UnicornValley.Domain.Common;
public abstract class Entity
{
    public Guid Id { get; }

    protected Entity(Guid id)
    {
        Id = id;
    }
}
