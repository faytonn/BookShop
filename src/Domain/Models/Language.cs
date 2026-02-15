namespace Domain.Models;

public sealed class Language : Entity<Guid>
{
    public required string Name { get; set; } = null!;
    public List<BookLanguage> Books { get; set; } = null!;
}