namespace Application.CQRS.Coupons.Features.DeleteCoupon;

public sealed class DeleteCouponEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/coupons/{id:guid}", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id)
    {
        await sender.Send(new DeleteCouponCommandRequest(id));
        return Results.NoContent();
    }
}
