namespace Application.CQRS.Orders.Features.CancelOrder;

public sealed class CancelOrderStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/v1/orders/cancel", Handler)
            .RequireAuthorization();
    }

    private static async Task<IResult> Handler(ISender sender, CancelOrderRequest request)
    {
        await sender.Send(new CancelOrderCommandRequest(request));
        return Results.NoContent();
    }

}
