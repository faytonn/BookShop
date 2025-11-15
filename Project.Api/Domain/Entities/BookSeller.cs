namespace Project.Api.Domain.Entities;

public sealed class BookSeller
{
    public Guid SellerId { get; set; }
    public Guid BookId { get; set; }
    public Seller Seller { get; set; }
    public Book Book { get; set; }
}
