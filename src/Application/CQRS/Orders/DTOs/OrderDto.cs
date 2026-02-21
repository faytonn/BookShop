namespace Application.CQRS.Orders.DTOs;

public record struct AddOrderRequest
    (
        Guid UserId,
        List<OrderItem> OrderItems,

        string? CouponCode
    );

public record struct OrderItem
    (
        Guid Id,
        int Quantity        
    );


public record struct AddOrderResponse
    (
         Guid Id,
         decimal TotalPrice,
         Guid UserId,
         List<OrderItem> OrderItems,
         string DisplayCode,
         string? CouponCode,
         DateTime CreatedAt
    );


public record struct AllOrdersDBModel
    (
        Guid Id,
        List<OrderItem> OrderItems,
        string? CouponCode,
        decimal TotalPrice,
        string DisplayCode,
        DateTime CreatedAt,
        Guid UserId,
        string? Name,
        string Email
    );

public record struct MyOrdersResponse
    (
        Guid Id,
        string Code,
        string? CouponCode,
        decimal TotalPrice,
        DateTime CreatedAt
    );

public record struct OrderDetailResponse
    (
        Guid Id,
        string Code,
        decimal TotalPrice,
        List<OrderItem> OrderItems,
        string? CouponCode,
        DateTime CreatedAt
    );
