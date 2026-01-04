namespace Project.Api.Application.Services.Abstractions;

public interface ICouponService 
{
    IEnumerable<CouponResponse> GetCoupons();
    Task<CouponResponse?> GetCouponAsync(Guid couponId);
    Task<CouponResponse?> GetCouponByCodeAsync(string code);
    Task<CouponResponse> CreateCouponAsync(CouponRequest request);
    Task<List<CouponResponse>> GenerateCouponsAsync(CouponGenerateRequest request);
    Task<CouponResponse?> UpdateCouponAsync(Guid couponId, CouponRequest request);
    Task<bool> ActivateCouponAsync(Guid couponId);
    Task<bool> DeactivateCouponAsync(Guid couponId);
    Task<bool> DeleteCouponAsync(Guid couponId);
}