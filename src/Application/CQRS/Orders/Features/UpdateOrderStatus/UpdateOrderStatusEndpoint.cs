namespace Application.CQRS.Orders.Features.UpdateOrderStatus;

public sealed class UpdateOrderStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/v1/orders/{id:guid}/status", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id, DTOs.UpdateOrderStatusRequest request)
    {
        var response = await sender.Send(new UpdateOrderStatusCommandRequest(id, request));
        return Results.Ok(response);
    }
}
