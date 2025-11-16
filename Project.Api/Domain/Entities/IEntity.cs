namespace Project.Api.Domain.Entities;

public interface IEntity;

public abstract class BaseEntity : IEntity
{
    public Guid Id { get; init; } = Guid.CreateVersion7();
}

public abstract class AuditableEntity : BaseEntity
{
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}