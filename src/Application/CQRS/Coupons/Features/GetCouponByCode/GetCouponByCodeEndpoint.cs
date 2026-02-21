namespace Application.CQRS.Coupons.Features.GetCouponByCode;

public sealed class GetCouponByCodeEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/coupons/code/{code}", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, string code)
    {
        var response = await sender.Send(new GetCouponByCodeQueryRequest(code));
        return Results.Ok(response.Coupon);
    }
}
