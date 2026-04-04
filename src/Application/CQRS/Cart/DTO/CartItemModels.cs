namespace Application.CQRS.Cart.DTO;

public record CartItemDto(Guid Id, Guid BookId, int Quantity/*, decimal TotalPrice*/);


public record AddCartItemDto(Guid BookId, int Quantity);