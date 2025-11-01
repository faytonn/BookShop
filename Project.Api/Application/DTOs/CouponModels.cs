namespace Project.Api.Application.DTOs;

public record struct CouponResponse(
    Guid Id,
    string Code,
    decimal Price,
    byte DiscountPercentage,
    DateTime ExpirationDate,
    int UsageLimit,
    int UsedCount,
    bool IsActive,
    DateTime CreatedAt
);

public record struct CouponRequest(
    decimal Price,
    byte DiscountPercentage,
    DateTime ExpirationDate,
    int UsageLimit
);

public record struct CouponGenerateRequest(
    int Count,
    decimal Price,
    byte DiscountPercentage,
    DateTime ExpirationDate,
    int UsageLimit
);
