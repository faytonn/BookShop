namespace Project.Api.Application.DTOs;

//public record OrderItemDTO
//    (
//        Guid BookId,
//        string Name,
//        decimal Price    //discounted or normal combined
//    );


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
         DateTime CreatedAt
    );


public record struct AllOrdersDBModel
    (
        Guid Id,
        List<OrderItem> OrderItems,
        string Code,
        decimal TotalPrice,
        DateTime CreatedAt,
        Guid UserId,
        string? Name,
        string Email
    );

public record struct MyOrdersResponse
    (
        Guid Id,
        List<OrderItem> OrderItems,
        string Code,
        decimal TotalPrice,
        DateTime CreatedAt
    );
