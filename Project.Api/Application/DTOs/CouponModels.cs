namespace Project.Api.Application.DTOs;

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
    byte DiscountPercentage,
    DateTime ExpirationDate,
    int UsageLimit
);

public record struct CouponGenerateRequest(
    int Count,
    byte DiscountPercentage,
    DateTime ExpirationDate,
    int UsageLimit
);
