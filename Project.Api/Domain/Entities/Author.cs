namespace Project.Api.Domain.Entities;

public class Author : AuditableEntity, ISoftDelete
{
    public string Name { get; set; }
    public List<Book> Books { get; set; } 


    public bool IsDeleted { get; set; }
}
