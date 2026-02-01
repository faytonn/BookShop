using Project.Api.Domain.Entities.Commons;

namespace Project.Api.Domain.Entities;

public sealed class Language : BaseEntity
{
    public required string Name { get; set; } = null!;
    public List<BookLanguage> Books { get; set; } = null!;
}
