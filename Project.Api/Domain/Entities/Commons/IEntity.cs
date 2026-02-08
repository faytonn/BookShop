namespace Project.Api.Domain.Entities.Commons;

public interface IEntity;

public abstract class Entity : IEntity;

public abstract class BaseEntity : Entity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
}

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; private set; }

    public AuditableEntity SetCreatedAt(DateTime date)
    {
        CreatedAt = date;
        return this;
    }
}

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}