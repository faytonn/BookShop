using Project.Api.Domain.Entities.Commons;

namespace Project.Api.Domain.Entities;

public class Seller : AuditableEntity, ISoftDelete
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public bool IsDeleted { get; set; }

    public UserRole Role = UserRole.Seller;
}

