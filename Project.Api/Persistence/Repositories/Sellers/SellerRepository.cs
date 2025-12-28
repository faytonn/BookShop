using Project.Api.Persistence.Repositories.Sellers;

public sealed class SellerRepository(AppDbContext context, IHttpContextAccessor contextAccessor) : Repository<Seller>(context, contextAccessor), ISellerRepository;