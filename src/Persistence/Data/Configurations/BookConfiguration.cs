namespace Persistence.Data.Configurations;

public class BookConfiguration(IServiceProvider provider) : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasIndex(b => b.Id);
        builder.Property(b => b.Name).HasMaxLength(128);

        using var scope = provider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (!context.Books.AsNoTracking().AnyAsync().GetAwaiter().GetResult())
        {
            var initialBooks = new Faker<Book>()
            .RuleFor(b => b.Id, () => Guid.CreateVersion7())
            .RuleFor(b => b.Name, faker => faker.Lorem.Sentence(wordCount: faker.Random.Int(3, 8)).Replace(".", ""))
            .RuleFor(b => b.Price, faker =>
            {
                var priceStr = faker.Commerce.Price(0.1m, 50);
                return decimal.Parse(priceStr);
            })
            .RuleFor(b => b.ReleaseDate, () => DateTime.UtcNow.AddDays(Random.Shared.Next(-365, 0)))
            .RuleFor(b => b.IsReleased, () => Random.Shared.Next(0, 1) > 0)
            .RuleFor(b => b.Stock, () => Random.Shared.Next(5, 150));

            builder.HasData(initialBooks.GenerateBetween(5, 20));
        }


    }
}