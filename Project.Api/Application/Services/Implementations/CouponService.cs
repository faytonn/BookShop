using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;
using Project.Api.Infrastucture.Providers.Coupons;
using Project.Api.Persistence.Repositories.Coupons;

namespace Project.Api.Application.Services;

public sealed class CouponService(AppDbContext context, CouponGenerator couponGenerator, ICouponRepository couponRepository) : ICouponService
{
    public IEnumerable<CouponResponse> GetCoupons()
    {
        var coupons = couponRepository
            .GetWhereAll(c => !c.IsDeleted)
            .Select(c => new CouponResponse(
                c.Id,
                c.Code,
                c.DiscountPercentage,
                c.ExpirationDate,
                c.UsageLimit,
                c.UsedCount,
                c.IsActive,
                c.CreatedAt
            ));

        return coupons;
    }

    public async Task<CouponResponse?> GetCouponAsync(Guid couponId)
    {
        var coupon = await context.Coupons
            .Where(c => c.Id == couponId && !c.IsDeleted)
            .Select(c => new CouponResponse(
                c.Id,
                c.Code,
                c.DiscountPercentage,
                c.ExpirationDate,
                c.UsageLimit,
                c.UsedCount,
                c.IsActive,
                c.CreatedAt
            ))
            .FirstOrDefaultAsync();

        if (coupon.Id == default)
            return null;

        return coupon;
    }

    public async Task<CouponResponse?> GetCouponByCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new ArgumentException("Coupon code is required.");

        var coupon = await context.Coupons
            .Where(c => c.Code == code.ToUpper() && !c.IsDeleted && c.IsActive)
            .Select(c => new CouponResponse(
                c.Id,
                c.Code,
                c.DiscountPercentage,
                c.ExpirationDate,
                c.UsageLimit,
                c.UsedCount,
                c.IsActive,
                c.CreatedAt
            ))
            .FirstOrDefaultAsync();

        if (coupon.Id == default)
            return null;

        return coupon;
    }

    public async Task<CouponResponse> CreateCouponAsync(CouponRequest request)
    {
        if (request.DiscountPercentage < 0 || request.DiscountPercentage > 100)
            throw new ArgumentException("Invalid discount values.");

        if (request.ExpirationDate <= DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be in the future.");

        if (request.UsageLimit <= 0)
            throw new ArgumentException("Usage limit must be greater than 0.");

        var code = couponGenerator.GenerateUniqueCouponCode(
            code => context.Coupons.Any(c => c.Code == code)
        );

        var newCoupon = new Coupon
        {
            Id = Guid.CreateVersion7(),
            Code = code,
            DiscountPercentage = request.DiscountPercentage,
            ExpirationDate = request.ExpirationDate,
            UsageLimit = request.UsageLimit,
            UsedCount = 0,
            IsActive = true,
        };

        context.Coupons.Add(newCoupon);
        await context.SaveChangesAsync();

        return new CouponResponse(
            newCoupon.Id,
            newCoupon.Code,
            newCoupon.DiscountPercentage,
            newCoupon.ExpirationDate,
            newCoupon.UsageLimit,
            newCoupon.UsedCount,
            newCoupon.IsActive,
            newCoupon.CreatedAt
        );
    }

    public async Task<List<CouponResponse>> GenerateCouponsAsync(CouponGenerateRequest request)
    {
        if (request.Count <= 0 || request.Count > 100)
            throw new ArgumentException("Count must be between 1 and 100.");

        if (request.DiscountPercentage < 0 || request.DiscountPercentage > 100)
            throw new ArgumentException("Invalid discount values.");

        if (request.ExpirationDate <= DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be in the future.");

        if (request.UsageLimit <= 0)
            throw new ArgumentException("Usage limit must be greater than 0.");

        var coupons = new List<Coupon>();

        for (int i = 0; i < request.Count; i++)
        {
            var code = couponGenerator.GenerateUniqueCouponCode(
                code => context.Coupons.Any(c => c.Code == code) || coupons.Any(c => c.Code == code)
            );

            var coupon = new Coupon
            {
                Id = Guid.CreateVersion7(),
                Code = code,
                DiscountPercentage = request.DiscountPercentage,
                ExpirationDate = request.ExpirationDate,
                UsageLimit = request.UsageLimit,
                UsedCount = 0,
                IsActive = true,
            };

            coupons.Add(coupon);
        }

        await context.Coupons.AddRangeAsync(coupons);
        await context.SaveChangesAsync();

        var responses = coupons.Select(c => new CouponResponse(
            c.Id,
            c.Code,
            c.DiscountPercentage,
            c.ExpirationDate,
            c.UsageLimit,
            c.UsedCount,
            c.IsActive,
            c.CreatedAt
        )).ToList();

        return responses;
    }

    public async Task<CouponResponse?> UpdateCouponAsync(Guid couponId, CouponRequest request)
    {
        var coupon = await context.Coupons
            .FirstOrDefaultAsync(c => c.Id == couponId && !c.IsDeleted);

        if (coupon is null)
            return null;

        if (request.DiscountPercentage < 0 || request.DiscountPercentage > 100)
            throw new ArgumentException("Invalid discount values.");

        if (request.ExpirationDate <= DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be in the future.");

        if (request.UsageLimit <= 0 || request.UsageLimit < coupon.UsedCount)
            throw new ArgumentException("Usage limit must be greater than 0 and not less than used count.");

        coupon.DiscountPercentage = request.DiscountPercentage;
        coupon.ExpirationDate = request.ExpirationDate;
        coupon.UsageLimit = request.UsageLimit;

        await context.SaveChangesAsync();

        return new CouponResponse(
            coupon.Id,
            coupon.Code,
            coupon.DiscountPercentage,
            coupon.ExpirationDate,
            coupon.UsageLimit,
            coupon.UsedCount,
            coupon.IsActive,
            coupon.CreatedAt
        );
    }

    public async Task<bool> ActivateCouponAsync(Guid couponId)
    {
        var coupon = await context.Coupons
            .FirstOrDefaultAsync(c => c.Id == couponId && !c.IsDeleted);

        if (coupon is null)
            return false;

        coupon.IsActive = true;
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeactivateCouponAsync(Guid couponId)
    {
        var coupon = await context.Coupons
            .FirstOrDefaultAsync(c => c.Id == couponId && !c.IsDeleted);

        if (coupon is null)
            return false;

        coupon.IsActive = false;
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCouponAsync(Guid couponId)
    {
        var coupon = await context.Coupons
            .FirstOrDefaultAsync(c => c.Id == couponId && !c.IsDeleted);

        if (coupon is null)
            return false;

        coupon.IsDeleted = true;
        await context.SaveChangesAsync();

        return true;
    }
}