namespace Application.CQRS.Orders.Features.GetOrderDetail;

public sealed class GetOrderDetailEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/orders/{id:guid}", Handler)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handler(ISender sender, Guid id)
    {
        var response = await sender.Send(new GetOrderDetailQueryRequest(id));
        return Results.Ok(response.Detail);
    }
}
