namespace Domain.Models;

public sealed class CartItem : AuditableEntity<Guid>
{
    public Guid UserId { get; set; }
    public Guid BookId { get; set; }
    public Book Book { get; set; } = null!;
    public Guid CartId  { get; set; }
    public int Quantity { get; set; }
}