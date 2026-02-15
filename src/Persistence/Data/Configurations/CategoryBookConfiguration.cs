namespace Persistence.Data.Configurations;
    public class CategoryBookConfiguration : IEntityTypeConfiguration<CategoryBook>
    {
        public void Configure(EntityTypeBuilder<CategoryBook> builder)
        {
            builder.HasIndex(cb => new {cb.BookId, cb.CategoryId });
        }
    }