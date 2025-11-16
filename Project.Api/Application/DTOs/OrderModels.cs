namespace Project.Api.Application.DTOs;

public record OrderItemDTO
    (
        Guid BookId,
        string Name,
        decimal Price    //discounted or normal combined
    );


public record struct AddOrderRequest
    (
        Guid UserId,
        List<Guid> OrderItemsIds,
        string? CouponCode
    );
