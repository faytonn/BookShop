using Azure;
using Project.Api.Application.Services;
using System.ComponentModel.DataAnnotations;

namespace Project.Api.Presentation.Endpoints;

public static class CouponEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapCouponEndpoints()
        {
            var group = route.MapGroup("api/v1/coupons");

            group.MapGet("", async (ICouponService couponService, IValidator<CouponRequest> validator) =>
            {
                var coupons = couponService.GetCoupons();
                return Results.Ok(coupons);
            });

            group.MapGet("{id}", async (ICouponService couponService, IValidator<CouponRequest> validator, string id) =>
            {
                if (!Guid.TryParse(id, out var couponId))
                    return Results.BadRequest("Invalid Coupon Id.");

                var coupon = await couponService.GetCouponAsync(couponId);

                if (coupon is null)
                    return Results.NotFound("Coupon not found.");

                return Results.Ok(coupon);
            });

            group.MapGet("code/{code}", async (ICouponService couponService, IValidator<CouponRequest> validator, string code) =>
            {
                try
                {
                    var coupon = await couponService.GetCouponByCodeAsync(code);

                    if (coupon is null)
                        return Results.NotFound("Coupon not found or inactive.");

                    return Results.Ok(coupon);
                }
                catch (ArgumentException e)
                {
                    return Results.BadRequest(e.Message);
                }
            });

            group.MapPost("", async (ICouponService couponService, IValidator<CouponRequest> validator, CouponRequest req) =>
            {
                var validation = validator.Validate(req);
                if (!validation.IsValid)
                    return Results.BadRequest(validation.Errors);
                try
                {
                    var response = await couponService.CreateCouponAsync(req);

                    return Results.Created(
                        $"api/v1/coupons/{response.Id}",
                        new { id = response.Id }
                    );
                }
                catch (ArgumentException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (DbUpdateException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }).RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));


            group.MapPut("{id:guid}", async (ICouponService couponService, Guid id, CouponRequest req) =>
            {
                try
                {
                    var response = await couponService.UpdateCouponAsync(id, req);

                    if (response is null)
                        return Results.NotFound("Coupon not found.");

                    return Results.Ok(response);
                }
                catch (ArgumentException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (DbUpdateException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }).RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));


            group.MapPatch("{id:guid}/activate", async (ICouponService couponService, Guid id) =>
            {
                try
                {
                    var activated = await couponService.ActivateCouponAsync(id);

                    if (!activated)
                        return Results.NotFound("Coupon not found.");

                    return Results.NoContent();
                }
                catch (DbUpdateException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }).RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));


            group.MapPatch("{id:guid}/deactivate", async (ICouponService couponService, Guid id) =>
            {
                try
                {
                    var deactivated = await couponService.DeactivateCouponAsync(id);

                    if (!deactivated)
                        return Results.NotFound("Coupon not found.");

                    return Results.NoContent();
                }
                catch (DbUpdateException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }).RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));


            group.MapDelete("{id:guid}", async (ICouponService couponService, Guid id) =>
            {
                try
                {
                    var deleted = await couponService.DeleteCouponAsync(id);

                    if (!deleted)
                        return Results.NotFound("Coupon not found.");

                    return Results.NoContent();
                }
                catch (DbUpdateException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }).RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
        }

    }
}
