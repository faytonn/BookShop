
namespace Project.Api.Persistence.Contexts.Configurations
{
    public class SellerConfiguration : IEntityTypeConfiguration<Seller>
    {
        public void Configure(EntityTypeBuilder<Seller> builder)
        {
            builder.HasIndex(s => s.Email);
        }
    }
}
