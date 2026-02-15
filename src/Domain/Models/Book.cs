namespace Domain.Models;

public sealed class Book : AuditableEntity<Guid>, ISoftDelete
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public byte Discount { get; set; }
    public DateTime ReleaseDate { get; set; }
    public bool IsReleased { get; set; }
    public bool IsDeleted { get; set; }


    public int Stock { get; set; }
    public bool IsAvailable => Stock > 0;


    public List<BookLanguage> Languages { get; set; }
    public List<CategoryBook> CategoryBooks { get; set; }
    public List<BookSeller> BookSellers { get; set; }
    public List<BookAuthor> Authors { get; set; }
}
