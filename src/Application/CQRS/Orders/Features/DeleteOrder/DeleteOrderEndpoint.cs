namespace Application.CQRS.Orders.Features.DeleteOrder;

public sealed class DeleteOrderEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/orders/{id:guid}", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id)
    {
        await sender.Send(new DeleteOrderCommandRequest(id));
        return Results.NoContent();
    }
}
