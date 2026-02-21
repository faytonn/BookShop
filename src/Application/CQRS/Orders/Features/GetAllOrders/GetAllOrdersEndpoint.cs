namespace Application.CQRS.Orders.Features.GetAllOrders;

public sealed class GetAllOrdersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/orders/all", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender)
    {
        var response = await sender.Send(new GetAllOrdersQueryRequest());
        return Results.Ok(response.Orders);
    }
}
