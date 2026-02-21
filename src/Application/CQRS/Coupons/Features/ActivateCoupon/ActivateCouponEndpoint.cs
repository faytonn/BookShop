namespace Application.CQRS.Coupons.Features.ActivateCoupon;

public sealed class ActivateCouponEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/v1/coupons/{id:guid}/activate", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id)
    {
        await sender.Send(new ActivateCouponCommandRequest(id));
        return Results.NoContent();
    }
}
