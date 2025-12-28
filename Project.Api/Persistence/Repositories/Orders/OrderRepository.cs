    using Project.Api.Persistence.Repositories.Orders;

    public sealed class OrderRepository(AppDbContext context, IHttpContextAccessor contextAccessor) : Repository<Order>(context, contextAccessor), IOrderRepository;