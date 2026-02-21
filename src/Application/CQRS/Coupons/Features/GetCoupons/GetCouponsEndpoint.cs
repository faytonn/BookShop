namespace Application.CQRS.Coupons.Features.GetCoupons;

public sealed class GetCouponsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/coupons", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender)
    {
        var response = await sender.Send(new GetCouponsQueryRequest());
        return Results.Ok(response.Coupons);
    }
}
