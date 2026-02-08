namespace Project.Api.Persistence.Contexts.Configurations;

public sealed class MetricConfiguration : IEntityTypeConfiguration<Metric>
{
    public void Configure(EntityTypeBuilder<Metric> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.MeasuredAt);
        builder.HasIndex(e => e.Key);
        
        builder.Property(e => e.Key)
            .HasMaxLength(128)
            .IsRequired();
            
        builder.Property(e => e.Value)
            .HasColumnType("jsonb")
            .IsRequired();
            
        builder.Property(e => e.MeasuredAt)
            .IsRequired();
    }
}
