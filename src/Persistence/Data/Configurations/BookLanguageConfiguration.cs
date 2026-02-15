namespace Persistence.Data.Configurations;

public class BookLanguageConfiguration : IEntityTypeConfiguration<BookLanguage>
{
    public void Configure(EntityTypeBuilder<BookLanguage> builder)
    {
        builder.HasIndex(bl => new { bl.BookId, bl.LanguageId });
    }
}