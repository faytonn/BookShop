namespace Project.Api.Domain.Entities;

public sealed class Seller : ISoftDelete
{
    public Guid UserId { get; private set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; private init; }

    public List<BookSeller> SellerBooks { get; set; }

    public Seller Create(Guid userId)
    {
        return new Seller
        {
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            IsDeleted = false
        };
    }
}
