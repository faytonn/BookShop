namespace Application.CQRS.Orders.DTOs;


// shipping address dto
public record struct ShippingAddressRequest
    (
        string FullAddress,
        string? City,
        string? Country,
        string? ZipCode,
        double? Longitude,
        double? Latitude
    );

public record struct ShippingAddressResponse
    (
        string FullAddress,
        string? City,
        string? Country,
        string? ZipCode,
        double? Longitude,
        double? Latitude
    );


// order items dto
public record struct OrderItem
    (
        Guid Id,
        int Quantity
    );


// add order dto
public record struct AddOrderRequest
    (
        List<OrderItem> OrderItems,
        ShippingAddressRequest ShippingAddress,
        string? CouponCode
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

// update order dto

public record struct UpdateOrderStatusRequest
    (
        OrderStatus NewStatus,
        string? Description,
        string? PictureUrl
    );

public record struct UpdateOrderStatusResponse
    (
        Guid OrderId,
        string DisplayCode,
        OrderStatus FromStatus,
        OrderStatus ToStatus,
        string? Description,
        DateTime ChangedAt
    );


// get all orders (admin) dto

public record struct AllOrdersDBModel
    (
        Guid Id,
        List<OrderItem> OrderItems,
        string? CouponCode,
        decimal TotalPrice,
        string DisplayCode,
        OrderStatus Status,
        DateTime CreatedAt,
        Guid UserId,
        string? Name,
        string Email
    );


// get my orders dto
public record struct MyOrdersResponse
    (
        Guid Id,
        string Code,
        string? CouponCode,
        decimal TotalPrice,
        OrderStatus Status,
        DateTime CreatedAt
    );



// get order details dto
public record struct OrderDetailResponse
    (
        Guid Id,
        string Code,
        decimal TotalPrice,
        OrderStatus Status,
        ShippingAddressResponse ShippingAddress,
        List<OrderItem> OrderItems,
        string? CouponCode,
        List<OrderHistoryResponse> OrderHistories,
        DateTime CreatedAt
    );


// get order history dtoo
public record struct OrderHistoryResponse
    (
        Guid Id,
        OrderStatus? FromStatus,
        OrderStatus ToStatus,
        Guid? ChangedByUserId,
        string? Description,
        string? PictureUrl,
        DateTime CreatedAt
    );
