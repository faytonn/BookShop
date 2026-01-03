using System.ComponentModel.DataAnnotations;

namespace Project.Api.Domain.Entities;

public sealed class BookLanguage : Entity
{
    public Guid BookId { get; set; }
    public Guid LanguageId { get; set; }

    public Book Book { get; init; } = null!;
    public Language Language { get; init; } = null!;
}
