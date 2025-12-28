using Project.Api.Persistence.Repositories.Sellers;

public sealed class SellerRepository(AppDbContext context) : Repository<Seller>(context), ISellerRepository;