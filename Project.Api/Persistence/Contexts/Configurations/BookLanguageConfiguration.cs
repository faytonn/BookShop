
namespace Project.Api.Persistence.Contexts.Configurations
{
    public class BookLanguageConfiguration : IEntityTypeConfiguration<BookLanguage>
    {
        public void Configure(EntityTypeBuilder<BookLanguage> builder)
        {
            builder.HasIndex(bl => new { bl.BookId, bl.LanguageId });
        }
    }
}
