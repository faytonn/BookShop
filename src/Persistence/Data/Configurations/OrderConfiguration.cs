namespace Persistence.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasIndex(o => o.UserId);

        builder.OwnsOne(o => o.ShippingAddress, sa =>
        {
            sa.ToJson();
        });

        builder.HasMany(o => o.OrderHistories).WithOne(oh => oh.Order).HasForeignKey(oh => oh.OrderId);
    }
}

public class OrderHistoryConfiguration : IEntityTypeConfiguration<OrderHistory>
{
    public void Configure(EntityTypeBuilder<OrderHistory> builder)
    {
        builder.HasIndex(oh => oh.OrderId);
        builder.HasOne(oh => oh.ChangedBy).WithMany().HasForeignKey(oh => oh.ChangedByUserId);
    }
}