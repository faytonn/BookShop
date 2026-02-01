namespace Project.Api.Presentation.Endpoints;


public static class CouponEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapCouponEndpoints()
        {
            var group = route.MapGroup("api/v1/coupons");

            group.MapGet("", async (ISender sender) =>
            {
                var response = await sender.Send(new GetCouponsQueryRequest());
                return Results.Ok(response.Coupons);
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));;

            group.MapGet("{id:guid}", async (ISender sender, Guid id) =>
            {
                var response = await sender.Send(new GetCouponByIdQueryRequest(id));
                return Results.Ok(response.Coupon);
            })
           .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!)); ;


            group.MapGet("code/{code}", async (ISender sender, string code) =>
            {
                var response = await sender.Send(new GetCouponByCodeQueryRequest(code));
                return Results.Ok(response.Coupon);
            })
           .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!)); 

            group.MapPost("", async (ISender sender, CouponRequest req) =>
            {
                var response = await sender.Send(new CreateCouponCommandRequest(req));
                return Results.Created($"/api/v1/coupons/{response.CouponId}", response);
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!)); ;


            group.MapPut("{id:guid}", async (ISender sender, Guid id, CouponRequest req) =>
            {
                var response = await sender.Send(new UpdateCouponCommandRequest(id, req));
                return Results.Ok(response);
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));

            group.MapPatch("{id:guid}/activate", async (ISender sender, Guid id) =>
            {
                await sender.Send(new ActivateCouponCommandRequest(id));
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!)); ;


            group.MapPatch("{id:guid}/deactivate", async (ISender sender, Guid id) =>
            {
                await sender.Send(new DeactivateCouponCommandRequest(id));
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!)); ;


            group.MapDelete("{id:guid}", async (ISender sender, Guid id) =>
            {
                await sender.Send(new DeleteCouponCommandRequest(id));
                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!)); ;

        }
    }
}