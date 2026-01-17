namespace Project.Api.Domain.Entities;

public class BookAuthor : Entity
{
    public Guid BookId { get; set; }
    public Guid AuthorId { get; set; }

    public Book Book { get; set; } = null!;
    public Author Author { get; set; } = null!;
}
