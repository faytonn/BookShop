using Project.Api.Domain.Entities.Commons;

namespace Project.Api.Domain.Entities;

public class Author : AuditableEntity, ISoftDelete
{
    public string Name { get; set; }
    public List<BookAuthor> Books { get; set; } 
    public bool IsDeleted { get; set; }
}
