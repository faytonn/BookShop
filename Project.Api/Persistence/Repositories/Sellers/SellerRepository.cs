using Project.Api.Persistence.Repositories.Sellers;

public sealed class SellerRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Seller>> logger) : Repository<Seller>(context, contextAccessor, logger), ISellerRepository;