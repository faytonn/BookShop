namespace Application.CQRS.Coupons.Features.DeactivateCoupon;

public sealed class DeactivateCouponEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/v1/coupons/{id:guid}/deactivate", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id)
    {
        await sender.Send(new DeactivateCouponCommandRequest(id));
        return Results.NoContent();
    }
}
