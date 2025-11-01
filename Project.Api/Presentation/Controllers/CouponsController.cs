using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Domain.Entities;
using Project.Api.Infrastucture.Providers.Coupons;
using Project.Api.Persistence.Contexts;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/coupons"), ApiController]
public sealed class CouponsController(AppDbContext context, CouponGenerator couponGenerator) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCoupons()
    {
        var coupons = context.Coupons
            .Where(c => !c.IsDeleted)
            .Select(c => new CouponResponse(
                c.Id,
                c.Code,
                c.Price,
                c.DiscountPercentage,
                c.ExpirationDate,
                c.UsageLimit,
                c.UsedCount,
                c.IsActive,
                c.CreatedAt
            ));

        return Ok(coupons);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCoupon(string id)
    {
        if (!Guid.TryParse(id, out var couponId))
            return BadRequest("Invalid Coupon Id.");

        var coupon = await context.Coupons
            .Where(c => c.Id == couponId && !c.IsDeleted)
            .Select(c => new CouponResponse(
                c.Id,
                c.Code,
                c.Price,
                c.DiscountPercentage,
                c.ExpirationDate,
                c.UsageLimit,
                c.UsedCount,
                c.IsActive,
                c.CreatedAt
            ))
            .FirstOrDefaultAsync();

        if (coupon.Id == default)
            return NotFound("Coupon not found.");

        return Ok(coupon);
    }

    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetCouponByCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BadRequest("Coupon code is required.");

        var coupon = await context.Coupons
            .Where(c => c.Code == code.ToUpper() && !c.IsDeleted && c.IsActive)
            .Select(c => new CouponResponse(
                c.Id,
                c.Code,
                c.Price,
                c.DiscountPercentage,
                c.ExpirationDate,
                c.UsageLimit,
                c.UsedCount,
                c.IsActive,
                c.CreatedAt
            ))
            .FirstOrDefaultAsync();

        if (coupon.Id == default)
            return NotFound("Coupon not found or inactive.");

        return Ok(coupon);
    }





    //admin methodlari

    [HttpPost/*, Authorize(Roles = "Admin,SuperAdmin")*/]
    public async Task<IActionResult> CreateCoupon(CouponRequest req)
    {
        if (req.Price < 0 || req.DiscountPercentage < 0 || req.DiscountPercentage > 100)
            return BadRequest("Invalid discount values.");

        if (req.ExpirationDate <= DateTime.UtcNow)
            return BadRequest("Expiration date must be in the future.");

        if (req.UsageLimit <= 0)
            return BadRequest("Usage limit must be greater than 0.");


        var code = couponGenerator.GenerateUniqueCouponCode(
            code => context.Coupons.Any(c => c.Code == code)
        );

        var newCoupon = new Coupon
        {
            Id = Guid.CreateVersion7(),
            Code = code,
            Price = req.Price,
            DiscountPercentage = req.DiscountPercentage,
            ExpirationDate = req.ExpirationDate,
            UsageLimit = req.UsageLimit,
            UsedCount = 0,
            IsActive = true,
        };

        try
        {
            context.Coupons.Add(newCoupon);
            await context.SaveChangesAsync();

            var response = new CouponResponse(
                newCoupon.Id,
                newCoupon.Code,
                newCoupon.Price,
                newCoupon.DiscountPercentage,
                newCoupon.ExpirationDate,
                newCoupon.UsageLimit,
                newCoupon.UsedCount,
                newCoupon.IsActive,
                newCoupon.CreatedAt
            );

            return CreatedAtAction(
                nameof(GetCoupon),
                new { id = newCoupon.Id },
                response
            );
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("generate")/*, Authorize(Roles = "Admin,SuperAdmin")*/]
    public async Task<IActionResult> GenerateCoupons(CouponGenerateRequest req)
    {
        if (req.Count <= 0 || req.Count > 100)
            return BadRequest("Count must be between 1 and 100.");

        if (req.Price < 0 || req.DiscountPercentage < 0 || req.DiscountPercentage > 100)
            return BadRequest("Invalid discount values.");

        if (req.ExpirationDate <= DateTime.UtcNow)
            return BadRequest("Expiration date must be in the future.");

        if (req.UsageLimit <= 0)
            return BadRequest("Usage limit must be greater than 0.");

        

        var coupons = new List<Coupon>();

        for (int i = 0; i < req.Count; i++)
        {
            var code = couponGenerator.GenerateUniqueCouponCode(
                code => context.Coupons.Any(c => c.Code == code) || coupons.Any(c => c.Code == code)
            );

            var coupon = new Coupon
            {
                Id = Guid.CreateVersion7(),
                Code = code,
                Price = req.Price,
                DiscountPercentage = req.DiscountPercentage,
                ExpirationDate = req.ExpirationDate,
                UsageLimit = req.UsageLimit,
                UsedCount = 0,
                IsActive = true,
            };

            coupons.Add(coupon);
        }

        try
        {
            await context.Coupons.AddRangeAsync(coupons);
            await context.SaveChangesAsync();

            var responses = coupons.Select(c => new CouponResponse(
                c.Id,
                c.Code,
                c.Price,
                c.DiscountPercentage,
                c.ExpirationDate,
                c.UsageLimit,
                c.UsedCount,
                c.IsActive,
                c.CreatedAt
            )).ToList();

            return CreatedAtAction(
                nameof(GetCoupons),
                responses
            );
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:guid}")/*, Authorize(Roles = "Admin,SuperAdmin")*/]
    public async Task<IActionResult> UpdateCoupon(Guid id, CouponRequest req)
    {
        var coupon = await context.Coupons
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (coupon is null)
            return NotFound("Coupon not found.");

        if (req.Price < 0 || req.DiscountPercentage < 0 || req.DiscountPercentage > 100)
            return BadRequest("Invalid discount values.");

        if (req.ExpirationDate <= DateTime.UtcNow)
            return BadRequest("Expiration date must be in the future.");

        if (req.UsageLimit <= 0 || req.UsageLimit < coupon.UsedCount)
            return BadRequest("Usage limit must be greater than 0 and not less than used count.");

       

        try
        {
            coupon.Price = req.Price;
            coupon.DiscountPercentage = req.DiscountPercentage;
            coupon.ExpirationDate = req.ExpirationDate;
            coupon.UsageLimit = req.UsageLimit;

            await context.SaveChangesAsync();

            var response = new CouponResponse(
                coupon.Id,
                coupon.Code,
                coupon.Price,
                coupon.DiscountPercentage,
                coupon.ExpirationDate,
                coupon.UsageLimit,
                coupon.UsedCount,
                coupon.IsActive,
                coupon.CreatedAt
            );

            return Ok(response);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id:guid}/activate")/*, Authorize(Roles = "Admin,SuperAdmin")*/]
    public async Task<IActionResult> ActivateCoupon(Guid id)
    {
        var coupon = await context.Coupons
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (coupon is null)
            return NotFound("Coupon not found.");

        try
        {
            coupon.IsActive = true;
            await context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("{id:guid}/deactivate")/*, Authorize(Roles = "Admin,SuperAdmin")*/]
    public async Task<IActionResult> DeactivateCoupon(Guid id)
    {
        var coupon = await context.Coupons
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (coupon is null)
            return NotFound("Coupon not found.");

        try
        {
            coupon.IsActive = false;
            await context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:guid}")/*, Authorize(Roles = "Admin,SuperAdmin")*/]
    public async Task<IActionResult> DeleteCoupon(Guid id)
    {
        var coupon = await context.Coupons
            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

        if (coupon is null)
            return NotFound("Coupon not found.");

        try
        {
            coupon.IsDeleted = true;
            await context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
