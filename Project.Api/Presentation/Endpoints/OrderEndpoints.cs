namespace Project.Api.Presentation.Endpoints;


public static class OrderEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapOrderEndpoints()
        {
            var group = route.MapGroup("api/v1/orders");

            group.MapPost("", async (ISender sender, AddOrderRequest request) =>
            {
                var response = await sender.Send(new AddOrderCommandRequest(request));
                return Results.Created($"/api/v1/orders/{response.Id}", response);
            })
            .RequireAuthorization();

            group.MapGet("all", async (ISender sender) =>
            {
                var response = await sender.Send(new GetAllOrdersQueryRequest());
                return Results.Ok(response.Orders);
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));

            group.MapGet("my", async (ISender sender) =>
            {
                var response = await sender.Send(new GetMyOrdersQueryRequest());
                return Results.Ok(response.MyOrders);
            })
            .RequireAuthorization();

            group.MapGet("{id:guid}", async (ISender sender, Guid id) =>
            {
                var response = await sender.Send(new GetOrderDetailQueryRequest(id));
                return Results.Ok(response.Detail);
            })
            .RequireAuthorization();

            group.MapDelete("{id:guid}", async (ISender sender, Guid id) =>
            {
                await sender.Send(new DeleteOrderCommandRequest(id));
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
        }
    }
}