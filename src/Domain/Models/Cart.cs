namespace Domain.Models;

public sealed class Cart : Entity<Guid>
{
    public Guid UserId { get; set; }
    public List<CartItem> Items { get; set; } = [];
}