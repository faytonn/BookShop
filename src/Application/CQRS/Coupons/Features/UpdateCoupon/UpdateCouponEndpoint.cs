namespace Application.CQRS.Coupons.Features.UpdateCoupon;

public sealed class UpdateCouponEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/coupons/{id:guid}", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id, DTOs.CouponRequest req)
    {
        var response = await sender.Send(new UpdateCouponCommandRequest(id, req));
        return Results.Ok(response);
    }
}
