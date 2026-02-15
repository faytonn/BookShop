namespace Domain.Models;

public class Seller : AuditableEntity<Guid>, ISoftDelete
{
    public required string Name { get; set; }
    public required string Surname { get; set; }
    public required string Email { get; set; }
    public bool IsDeleted { get; set; }

    public UserRole Role = UserRole.Seller;
}
