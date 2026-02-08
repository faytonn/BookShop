using Bogus;

namespace Project.Api.Persistence.Contexts.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasIndex(b => b.Id);
            builder.Property(b => b.Name).HasMaxLength(128);

            var initialBooks = new Faker<Book>()
                .RuleFor(b => b.Id, () => Guid.CreateVersion7())
                .RuleFor(b => b.Name, faker => faker.Lorem.Sentence(wordCount: faker.Random.Int(3, 8)).Replace(".", ""))
                .RuleFor(b => b.Price, faker =>
                {
                    var priceStr = faker.Commerce.Price(0.1m, 50);
                    return decimal.Parse(priceStr);
                })
                .RuleFor(b => b.ReleaseDate, () => DateTime.Now.AddDays(Random.Shared.Next(-365, 0)))
                .RuleFor(b => b.IsReleased, () => Random.Shared.Next(0, 1) > 0)
                .RuleFor(b => b.Stock, () => Random.Shared.Next(5, 150));

            builder.HasData(initialBooks.GenerateBetween(5, 20));
        }
    }
}
