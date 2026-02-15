namespace Shared.Domain;

public interface IEntity;

public abstract class Entity : IEntity;

public abstract class Entity<T> : Entity
{
    public required T Id { get; init; }
}

public interface IAuditableEntity
{
    DateTime CreatedAt { get; set; }
    IAuditableEntity SetCreatedAt(DateTime date);
}

public abstract class AuditableEntity<T> : Entity<T>, IAuditableEntity
{
    public DateTime CreatedAt { get; set; }

    public IAuditableEntity SetCreatedAt(DateTime date)
    {
        CreatedAt = date;
        return this;
    }
}

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}