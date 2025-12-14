using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/coupons"), ApiController]
public sealed class CouponsController(ICouponService couponService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCoupons()
    {
        var coupons = couponService.GetCoupons();
        return Ok(coupons);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCoupon(string id)
    {
        if (!Guid.TryParse(id, out var couponId))
            return BadRequest("Invalid Coupon Id.");

        var coupon = await couponService.GetCouponAsync(couponId);

        if (coupon is null)
            return NotFound("Coupon not found.");

        return Ok(coupon);
    }

    [HttpGet("code/{code}")]
    public async Task<IActionResult> GetCouponByCode(string code)
    {
        try
        {
            var coupon = await couponService.GetCouponByCodeAsync(code);

            if (coupon is null)
                return NotFound("Coupon not found or inactive.");

            return Ok(coupon);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost, Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> CreateCoupon(CouponRequest req)
    {
        try
        {
            var response = await couponService.CreateCouponAsync(req);

            return CreatedAtAction(
                nameof(GetCoupon),
                new { id = response.Id },
                response
            );
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
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

    [HttpPost("generate"), Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> GenerateCoupons(CouponGenerateRequest req)
    {
        try
        {
            var responses = await couponService.GenerateCouponsAsync(req);

            return CreatedAtAction(
                nameof(GetCoupons),
                responses
            );
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
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

    [HttpPut("{id:guid}"), Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateCoupon(Guid id, CouponRequest req)
    {
        try
        {
            var response = await couponService.UpdateCouponAsync(id, req);

            if (response is null)
                return NotFound("Coupon not found.");

            return Ok(response);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
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

    [HttpPatch("{id:guid}/activate"), Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> ActivateCoupon(Guid id)
    {
        try
        {
            var activated = await couponService.ActivateCouponAsync(id);

            if (!activated)
                return NotFound("Coupon not found.");

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

    [HttpPatch("{id:guid}/deactivate"), Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeactivateCoupon(Guid id)
    {
        try
        {
            var deactivated = await couponService.DeactivateCouponAsync(id);

            if (!deactivated)
                return NotFound("Coupon not found.");

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

    [HttpDelete("{id:guid}"), Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeleteCoupon(Guid id)
    {
        try
        {
            var deleted = await couponService.DeleteCouponAsync(id);

            if (!deleted)
                return NotFound("Coupon not found.");

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