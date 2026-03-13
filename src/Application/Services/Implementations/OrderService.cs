namespace Application.Services.Implementations;

public sealed class OrderService(IUnitOfWork unitOfWork, IHttpContextAccessor accessor) : IOrderService
{
    private readonly ClaimsPrincipal User = accessor.HttpContext?.User ?? throw new InvalidDataException("No user found.");

    private static string GenerateDisplayCode(Guid id) => $"ORDER-{id.ToString("N")[..8].ToUpper()}";

    private static readonly Dictionary<OrderStatus, OrderStatus[]> AllowedTransitions = new()
    {
        { OrderStatus.Pending,        [OrderStatus.Confirmed, OrderStatus.Canceled] },
        { OrderStatus.Confirmed,      [OrderStatus.Shipped, OrderStatus.Canceled] },
        { OrderStatus.Shipped,        [OrderStatus.OutForDelivery] },
        { OrderStatus.OutForDelivery, [OrderStatus.Delivered] },    };

    private static bool IsValidTransition(OrderStatus from, OrderStatus to)
        => AllowedTransitions.TryGetValue(from, out var allowed) && allowed.Contains(to);


    public async Task<AddOrderResponse> AddOrderAsync(AddOrderRequest request)
    {
        if (request.OrderItems is null || request.OrderItems.Count == 0)
            throw new NullReferenceException("No items provided.");

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new InvalidDataException("Invalid or missing token.");

        var user = await unitOfWork.Users.FindAsync(userId);
        if (user is null || user.IsDeleted)
            throw new InvalidDataException("User not found or inactive.");

        Coupon? coupon = null;

        if (request.CouponCode is not null)
        {
            coupon = await unitOfWork.Coupons
              .GetWhereAll(c => c.Code == request.CouponCode && c.IsActive && !c.IsDeleted)
              .FirstOrDefaultAsync();

            if (coupon is null)
                throw new NullReferenceException("The following coupon was not found.");

            if (coupon.ExpirationDate < DateTime.UtcNow)
                throw new InvalidOperationException("The coupon is expired.");

            if (coupon.UsageLimit <= coupon.UsedCount)
                throw new InvalidOperationException("The usage limit is exceeded.");
        }

        var booksTable = unitOfWork.Books
            .GetWhereAll(b => request.OrderItems.Select(oi => oi.Id)
            .Contains(b.Id) && !b.IsDeleted, tracking: true);

        var books = await booksTable
            .Select(b => new BookOrderDTO
            (
                b.Id,
                b.Name,
                decimal.Round(b.Price - (b.Price * b.Discount / 100), 2, MidpointRounding.ToPositiveInfinity),
                b.Stock
            ))
            .ToArrayAsync();

        decimal totalPrice = 0;

        foreach (var book in books)
        {
            foreach (var req in request.OrderItems)
            {
                if (req.Id == book.Id)
                {
                    if (req.Quantity > book.Stock)
                        throw new InvalidOperationException("The desired quantity of this product exceeds our current stock.");

                    totalPrice = decimal.Round(totalPrice + book.Price * req.Quantity * (coupon is not null ? 1 - ((decimal)coupon.DiscountPercentage / 100) : 1), 2, MidpointRounding.ToPositiveInfinity);

                }
            }
        }

        var newOrderId = Guid.CreateVersion7();

        var shippingAddress = new ShippingAddress
        {
            FullAddress = request.ShippingAddress.FullAddress,
            City = request.ShippingAddress.City,
            Country = request.ShippingAddress.Country,
            ZipCode = request.ShippingAddress.ZipCode,
            Longitude = request.ShippingAddress.Longitude,
            Latitude = request.ShippingAddress.Latitude
        };

        var newOrder = new Order
        {
            Id = newOrderId,
            OrderItems = JsonSerializer.Serialize(request.OrderItems),
            DisplayCode = GenerateDisplayCode(newOrderId),
            CouponCode = request.CouponCode,
            TotalPrice = totalPrice,
            UserId = userId,
            ShippingAddress = shippingAddress,
            OrderHistories =
            [
                new OrderHistory
                {
                    Id = Guid.CreateVersion7(),
                    OrderId = newOrderId,
                    FromStatus = null,
                    ToStatus = OrderStatus.Pending,
                    ChangedByUserId = userId,
                    Description = "Order created"
                }
            ]
        };

        await using var transaction = await unitOfWork.Orders.BeginTransactionAsync();

        try
        {
            await unitOfWork.Orders.AddAsync(newOrder);

            var booksToUpdate = await booksTable.ToListAsync();

            foreach (var book in booksToUpdate)
            {
                foreach (var req in request.OrderItems)
                {
                    if (req.Id == book.Id)
                        book.Stock -= req.Quantity;
                }
            }

            if (coupon is not null)
                coupon.UsedCount += 1;

            await unitOfWork.Orders.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

        return new AddOrderResponse(
            newOrder.Id,
            newOrder.TotalPrice,
            newOrder.UserId,
            JsonSerializer.Deserialize<List<OrderItem>>(newOrder.OrderItems)!,
            GenerateDisplayCode(newOrder.Id),
            newOrder.CouponCode,
            newOrder.CreatedAt);
    }



    public async Task<UpdateOrderStatusResponse> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequest request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new InvalidDataException("Invalid or missing token.");

        var order = await unitOfWork.Orders
            .GetWhereAll(o => o.Id == orderId && !o.IsDeleted, tracking: true)
            .FirstOrDefaultAsync();

        if (order is null)
            throw new NullReferenceException("Order not found.");

        var fromStatus = order.Status;
        var toStatus = request.NewStatus;

        if (!IsValidTransition(fromStatus, toStatus))
            throw new InvalidOperationException(
                $"Cannot transition from {fromStatus} to {toStatus}. " +
                $"Allowed transitions from {fromStatus}: " +
                $"{(AllowedTransitions.TryGetValue(fromStatus, out var a) ? string.Join(", ", a) : "none (terminal status)")}.");

        order.Status = toStatus;

        var history = new OrderHistory
        {
            Id = Guid.CreateVersion7(),
            OrderId = orderId,
            FromStatus = fromStatus,
            ToStatus = toStatus,
            ChangedByUserId = userId,
            Description = request.Description,
            PictureUrl = request.PictureUrl
        };

        await unitOfWork.OrderHistories.AddAsync(history);
        await unitOfWork.Orders.SaveChangesAsync();

        return new UpdateOrderStatusResponse(
            order.Id,
            order.DisplayCode,
            fromStatus,
            toStatus,
            request.Description,
            history.CreatedAt);
    }


    public IEnumerable<AllOrdersDBModel> GetAllOrders()
    {
        return unitOfWork.Orders
             .GetOrderWithUser()
             .Select(o => new AllOrdersDBModel
             (
                 o.Id,
                 JsonSerializer.Deserialize<List<OrderItem>>(o.OrderItems)!,
                 o.CouponCode,
                 o.TotalPrice,
                 o.DisplayCode,
                 o.Status,
                 o.CreatedAt,
                 o.UserId,
                 o.User.Name,
                 o.User.Email
             ));
    }


    public IEnumerable<MyOrdersResponse> GetMyOrders()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new InvalidDataException("Invalid or missing token.");

        var userExists = unitOfWork.Users.GetWhereAll(u => u.Id == userId && !u.IsDeleted)
                                        .Any();
        if (!userExists)
            throw new InvalidDataException("User not found or inactive.");

        return unitOfWork.Orders
            .GetWhereAll(o => o.UserId == userId && !o.IsDeleted)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new MyOrdersResponse
            (
                o.Id,
                o.DisplayCode,
                o.CouponCode,
                o.TotalPrice,
                o.Status,
                o.CreatedAt
            ));
    }


    public async Task<OrderDetailResponse> GetOrderDetailAsync(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new InvalidDataException("Invalid or missing token.");

        var order = await unitOfWork.Orders
            .GetWhereAll(o => o.Id == id && !o.IsDeleted)
            .Include(o => o.OrderHistories)
            .FirstOrDefaultAsync();

        if (order is null)
            throw new NullReferenceException("Order not found.");

        var items = JsonSerializer.Deserialize<List<OrderItem>>(order.OrderItems) ?? [];

        var histories = order.OrderHistories
            .OrderBy(h => h.CreatedAt)
            .Select(h => new OrderHistoryResponse(
                h.Id,
                h.FromStatus,
                h.ToStatus,
                h.ChangedByUserId,
                h.Description,
                h.PictureUrl,
                h.CreatedAt
            ))
            .ToList();

        var shippingAddress = new ShippingAddressResponse(
            order.ShippingAddress.FullAddress,
            order.ShippingAddress.City,
            order.ShippingAddress.Country,
            order.ShippingAddress.ZipCode,
            order.ShippingAddress.Longitude,
            order.ShippingAddress.Latitude
        );

        return new OrderDetailResponse(
            order.Id,
            order.DisplayCode,
            order.TotalPrice,
            order.Status,
            shippingAddress,
            items,
            order.CouponCode,
            histories,
            order.CreatedAt
        );
    }

    public async Task DeleteOrderAsync(Guid orderId)
    {
        var order = await unitOfWork.Orders.GetWhereAll(o => o.Id == orderId, tracking: true).FirstOrDefaultAsync();

        if (order is null)
            throw new NullReferenceException("Order not found.");

        order.IsDeleted = true;
        await unitOfWork.Orders.SaveChangesAsync();
    }
}
