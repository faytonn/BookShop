namespace Domain.Models;

public sealed class Category : Entity<Guid>
{
    public string Name { get; set; } = null!;
    public int PriorityLevel { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public Category ParentCategory { get; set; }

    public List<CategoryBook> Books { get; set; }
}