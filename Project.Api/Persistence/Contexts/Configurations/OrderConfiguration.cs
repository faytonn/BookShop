
namespace Project.Api.Persistence.Contexts.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasIndex(o => o.UserId);

        }
    }
}
