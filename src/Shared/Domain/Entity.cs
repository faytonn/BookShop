namespace Shared.Domain;

public interface IEntity;

public abstract class Entity : IEntity;

public abstract class Entity<T> : Entity
{
    public required T Id { get; init; }
}

public abstract class AuditableEntity<T> : Entity<T>
{
    public DateTime CreatedAt { get; private set; }

    public AuditableEntity<T> SetCreatedAt(DateTime date)
    {
        CreatedAt = date;
        return this;
    }
}

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}