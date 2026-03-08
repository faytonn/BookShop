using Microsoft.Extensions.Logging;

namespace Persistence.Repositories.OrderHistories;

public sealed class OrderHistoryRepository(AppDbContext context, IHttpContextAccessor contextAccessor, ILogger<Repository<OrderHistory>> logger) : Repository<OrderHistory>(context, contextAccessor, logger), IOrderHistoryRepository
{

}