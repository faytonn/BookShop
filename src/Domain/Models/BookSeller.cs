namespace Domain.Models;

public sealed class BookSeller : Entity
{
    public Guid SellerId { get; set; }
    public Guid BookId { get; set; }
    public User Seller { get; set; }
    public Book Book { get; set; }
}

