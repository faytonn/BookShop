namespace Application.CQRS.Coupons.Features.GetCouponById;

public sealed class GetCouponByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/coupons/{id:guid}", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id)
    {
        var response = await sender.Send(new GetCouponByIdQueryRequest(id));
        return Results.Ok(response.Coupon);
    }
}
