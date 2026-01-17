
namespace Project.Api.Persistence.Contexts.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasIndex(a => a.Name)
                    .IsUnique();
        }
    }
}
