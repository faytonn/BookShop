namespace Project.Api.Presentation.Endpoints;

public static class OrderEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapOrderEndpoints()
        {
            var group = route.MapGroup("api/v1/orders");

            group.MapPost("", async (IOrderService orderService, AddOrderRequest request) =>
            {
                try
                {
                    var newOrder = await orderService.AddOrderAsync(request);
                    return Results.Created($"/api/v1/orders/{newOrder.Id}", newOrder);
                }
                catch (NullReferenceException ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.NotFound);
                }
                catch (InvalidDataException ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.Unauthorized);
                }
                catch (Exception ex) when (ex is InvalidOperationException || ex is DbUpdateException)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
                }
            }).RequireAuthorization();


            group.MapGet("my", (IOrderService orderService, IMemoryCache cache, ClaimsPrincipal user) =>
            {
                try
                {
                    var userId = user.FindFirstValue(ClaimTypes.NameIdentifier)
                        ?? throw new InvalidOperationException("User id claim not found");

                    var result = cache.GetOrCreate($"orders:user:{userId}", entry =>
                    {
                        entry.AbsoluteExpiration = DateTime.Now.AddMinutes(10);
                        return orderService.GetMyOrders();
                    });

                    return Results.Ok(result);
                }
                catch (InvalidDataException ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.Unauthorized);
                }
                catch (Exception ex)
                {
                    return Results.Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
                }
            }).RequireAuthorization();


            group.MapGet("all", (IOrderService orderService) =>
            {
                var allOrders = orderService.GetAllOrders();
                return Results.Ok(allOrders);
            }).RequireAuthorization(policy =>
                policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!)
            );


            group.MapGet("{id:guid}", async (AppDbContext context, IMemoryCache cache, ClaimsPrincipal user, Guid id) =>
            {
                static string GenerateDisplayCode(Guid id) => $"ORDER{id.ToString("N")[..8].ToUpper()}";

                var userIdClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                    return Results.Unauthorized();

                var order = await cache.GetOrCreateAsync($"order:detail:{id}", async entry =>
                {
                    entry.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
                    return await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                });

                if (order is null)
                    return Results.NotFound("Order not found.");

                var role = user.FindFirstValue(ClaimTypes.Role);

                if (role != "Admin" && role != "SuperAdmin" && order.UserId != userId)
                    return Results.Forbid();

                var items = JsonSerializer.Deserialize<BookOrderDTO[]>(order.OrderItems) ?? [];

                var response = new
                {
                    order.Id,
                    Code = GenerateDisplayCode(order.Id),
                    order.TotalPrice,
                    order.CreatedAt,
                    Items = items
                };

                return Results.Ok(response);
            }).RequireAuthorization();

            group.MapDelete("{id:guid}", async (AppDbContext context, Guid id) =>
            {
                var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
                if (order is null)
                    return Results.NotFound("Order not found.");

                try
                {
                    context.Orders.Remove(order);
                    await context.SaveChangesAsync();
                    return Results.NoContent();
                }
                catch (DbUpdateException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }).RequireAuthorization(policy =>
                policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!)
            );
        }
    }
}
