namespace Project.Api.Persistence.Repositories.Orders;

public interface IOrderRepository : IRepository<Order>
{
    public IQueryable<Order> GetOrderWithUser();
}

