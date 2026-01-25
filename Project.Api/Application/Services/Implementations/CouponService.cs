namespace Project.Api.Application.Services;

public sealed class CouponService(IUnitOfWork unitOfWork, CouponGenerator couponGenerator) : ICouponService
{
    public IEnumerable<CouponResponse> GetCoupons()
    {
        var coupons = unitOfWork.Coupons
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
        var coupon = await unitOfWork.Coupons
            .GetWhereAll(c => c.Id == couponId && !c.IsDeleted)
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

        var coupon = await unitOfWork.Coupons
            .GetWhereAll(c => c.Code == code.ToUpper() && !c.IsDeleted && c.IsActive)
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
            code => unitOfWork.Coupons.CodeExistsAsync(code).Result
        );

        var newCoupon = new Coupon
        {
            Id = Guid.CreateVersion7(),
            Code = code,
            DiscountPercentage = (byte)request.DiscountPercentage,
            ExpirationDate = request.ExpirationDate,
            UsageLimit = request.UsageLimit,
            UsedCount = 0,
            IsActive = true,
        };

        await unitOfWork.Coupons.AddAsync(newCoupon);
        await unitOfWork.SaveChangesAsync();

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

    //public async Task<List<CouponResponse>> GenerateCouponsAsync(CouponGenerateRequest request)
    //{
    //    if (request.Count <= 0 || request.Count > 100)
    //        throw new ArgumentException("Count must be between 1 and 100.");

    //    if (request.DiscountPercentage < 0 || request.DiscountPercentage > 100)
    //        throw new ArgumentException("Invalid discount values.");

    //    if (request.ExpirationDate <= DateTime.UtcNow)
    //        throw new ArgumentException("Expiration date must be in the future.");

    //    if (request.UsageLimit <= 0)
    //        throw new ArgumentException("Usage limit must be greater than 0.");

    //    var coupons = new List<Coupon>();

    //    for (int i = 0; i < request.Count; i++)
    //    {
    //        var code = couponGenerator.GenerateUniqueCouponCode(
    //            code => unitOfWork.Coupons.CodeExistsAsync(code).Result || coupons.Any(c => c.Code == code)
    //        );

    //        var coupon = new Coupon
    //        {
    //            Id = Guid.CreateVersion7(),
    //            Code = code,
    //            DiscountPercentage = (byte)request.DiscountPercentage,
    //            ExpirationDate = request.ExpirationDate,
    //            UsageLimit = request.UsageLimit,
    //            UsedCount = 0,
    //            IsActive = true,
    //        };

    //        coupons.Add(coupon);
    //    }

    //    await unitOfWork.Coupons.AddRangeAsync(coupons);
    //    await unitOfWork.SaveChangesAsync();

    //    var responses = coupons.Select(c => new CouponResponse(
    //        c.Id,
    //        c.Code,
    //        c.DiscountPercentage,
    //        c.ExpirationDate,
    //        c.UsageLimit,
    //        c.UsedCount,
    //        c.IsActive,
    //        c.CreatedAt
    //    )).ToList();

    //    return responses;
    //}

    public async Task<CouponResponse?> UpdateCouponAsync(Guid couponId, CouponRequest request)
    {
        var coupon = await unitOfWork.Coupons
            .FindAsync(couponId);

        if (coupon is null || coupon.IsDeleted)
            return null;

        if (request.DiscountPercentage < 0 || request.DiscountPercentage > 100)
            throw new ArgumentException("Invalid discount values.");

        if (request.ExpirationDate <= DateTime.UtcNow)
            throw new ArgumentException("Expiration date must be in the future.");

        if (request.UsageLimit <= 0 || request.UsageLimit < coupon.UsedCount)
            throw new ArgumentException("Usage limit must be greater than 0 and not less than used count.");

        coupon.DiscountPercentage = (byte)request.DiscountPercentage;
        coupon.ExpirationDate = request.ExpirationDate;
        coupon.UsageLimit = request.UsageLimit;

        unitOfWork.Coupons.Update(coupon);   
        await unitOfWork.SaveChangesAsync();

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
        var coupon = await unitOfWork.Coupons
            .FindAsync(couponId);

        if (coupon is null || coupon.IsDeleted)
            return false;

        coupon.IsActive = true;
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeactivateCouponAsync(Guid couponId)
    {
        var coupon = await unitOfWork.Coupons
            .FindAsync(couponId);

        if (coupon is null || coupon.IsDeleted)
            return false;

        coupon.IsActive = false;
        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteCouponAsync(Guid couponId)
    {
        var coupon = await unitOfWork.Coupons
            .FindAsync(couponId);

        if (coupon is null || coupon.IsDeleted)
            return false;

        coupon.IsDeleted = true;
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}