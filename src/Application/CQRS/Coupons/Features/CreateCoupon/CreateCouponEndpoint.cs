namespace Application.CQRS.Coupons.Features.CreateCoupon;

public sealed class CreateCouponEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/coupons", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, DTOs.CouponRequest req)
    {
        var response = await sender.Send(new CreateCouponCommandRequest(req));
        return Results.Created($"/api/v1/coupons/{response.CouponId}", response);
    }
}
