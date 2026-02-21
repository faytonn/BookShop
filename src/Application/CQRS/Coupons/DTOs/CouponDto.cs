namespace Application.CQRS.Coupons.DTOs;

public record struct CouponResponse(
    Guid Id,
    string Code,
    byte DiscountPercentage,
    DateTime ExpirationDate,
    int UsageLimit,
    int UsedCount,
    bool IsActive,
    DateTime CreatedAt
);

public record struct CouponRequest(
    int DiscountPercentage,
    DateTime ExpirationDate,
    int UsageLimit
);
