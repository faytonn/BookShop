namespace Application.CQRS.Orders.Features.GetMyOrders;

public sealed class GetMyOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/orders/my", Handler)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handler(ISender sender)
    {
        var response = await sender.Send(new GetMyOrdersQueryRequest());
        return Results.Ok(response.MyOrders);
    }
}
