namespace Project.Api.Domain.Entities;

public sealed class BookSeller
{
    public Guid SellerId { get; set; }
    public Guid BookId { get; set; }
    public User Seller { get; set; }
    public Book Book { get; set; }
}
