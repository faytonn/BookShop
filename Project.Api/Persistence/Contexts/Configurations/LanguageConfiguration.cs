
namespace Project.Api.Persistence.Contexts.Configurations
{
    public class LanguageConfiguration : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasIndex(l => l.Name);
        }
    }
}
