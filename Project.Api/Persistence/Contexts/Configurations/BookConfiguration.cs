
namespace Project.Api.Persistence.Contexts.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasIndex(b => b.Id);
            builder.Property(b => b.Name).HasMaxLength(128);
        }
    }
}
