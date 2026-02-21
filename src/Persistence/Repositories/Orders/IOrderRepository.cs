namespace Persistence.Repositories.Orders;

public interface IOrderRepository : Repositories.Shared.IRepository<Order>
{
    public IQueryable<Order> GetOrderWithUser();
}

