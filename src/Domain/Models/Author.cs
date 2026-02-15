namespace Domain.Models;

public class Author : AuditableEntity<Guid>, ISoftDelete
{
    public string Name { get; set; }
    public List<BookAuthor> Books { get; set; }
    public bool IsDeleted { get; set; }
}
