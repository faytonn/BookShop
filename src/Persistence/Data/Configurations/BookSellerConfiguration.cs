namespace Persistence.Data.Configurations;

public class BookSellerConfiguration : IEntityTypeConfiguration<BookSeller>
{
    public void Configure(EntityTypeBuilder<BookSeller> builder)
    {
        builder.HasIndex(bs => new { bs.BookId, bs.SellerId });
    }
}