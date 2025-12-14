namespace Project.Api.Domain.Entities;

public sealed class Book : AuditableEntity, ISoftDelete
{
    public required string Name { get; set; }
    public decimal Price { get; set; }
    public byte Discount { get; set; }
    public DateTime ReleaseDate { get; set; }
    public bool IsReleased { get; set; }
    public bool IsDeleted { get; set; }

    public int Stock { get; set; }
    public bool IsAvailable { get; set; }


    public List<BookLanguage> Languages { get; set; }
    public List<CategoryBook> CategoryBooks { get; set; }
    public List<BookSeller> BookSellers { get; set; }
}