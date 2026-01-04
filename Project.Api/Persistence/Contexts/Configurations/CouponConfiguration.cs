
namespace Project.Api.Persistence.Contexts.Configurations
{
    public sealed class CouponConfiguration : IEntityTypeConfiguration<Coupon>
    {
        public void Configure(EntityTypeBuilder<Coupon> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.Code).IsUnique();

            builder.Property(e => e.Code).HasMaxLength(8);
        }
    }
}
