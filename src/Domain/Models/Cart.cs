namespace Domain.Models;

public sealed class Cart : Entity<Guid>
{
    public Guid UserId { get; set; }
    public ICollection<CartItem> Items { get; set; } = [];
}