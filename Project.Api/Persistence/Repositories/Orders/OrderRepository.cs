    using Project.Api.Persistence.Repositories.Orders;

    public sealed class OrderRepository(AppDbContext context) : Repository<Order>(context), IOrderRepository;