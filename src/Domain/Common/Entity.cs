namespace UnicornValley.Domain.Common;
public abstract class Entity
{
    public abstract Guid Id { get; protected set; }
}