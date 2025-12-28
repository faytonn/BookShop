    using Project.Api.Persistence.Repositories.Orders;

    public sealed class OrderRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<Order>> logger) : Repository<Order>(context, contextAccessor, logger), IOrderRepository;