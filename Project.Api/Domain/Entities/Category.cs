namespace Project.Api.Domain.Entities;

public class Category : BaseEntity
{
    public string Name { get; set; } = null!;
    public int PriorityLevel { get; set; }
    public Guid ParentId { get; set; }
    public Category Parent { get; set; }

    public List<CategoryBook> Books { get; set; } 
}