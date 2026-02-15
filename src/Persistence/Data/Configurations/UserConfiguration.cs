namespace Persistence.Data.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Email).IsUnique();

        builder.Property(e => e.Name).HasMaxLength(128);
        builder.Property(e => e.Surname).HasMaxLength(128);
        builder.Property(e => e.Email).HasMaxLength(256);
    }
}
