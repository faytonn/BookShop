namespace Domain.Models;

public sealed class User : AuditableEntity<Guid>, ISoftDelete
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public required string Email { get; set; }
    public required string HashedPassword { get; set; }
    public UserRole Role { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? LastLoggedAt { get; set; }
    public List<Order> Orders { get; set; }
}

public enum UserRole : byte
{
    None = 0,
    Seller = 1,
    Admin = 2,
    SuperAdmin = 3
}