namespace Application.CQRS.Cart.Features.AddCartItem;

public sealed class AddCartItemEndpoint : ICarterModule
{
    public record AddCartItemRequest(List<AddCartItemDto> Items);

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/cart/{userId}/items", Handler);
    }

    private static async Task<IResult> Handler(Guid userId, AddCartItemRequest request, ISender sender)
    {
        var response = await sender.Send(new AddCartItemCommandRequest(userId, request.Items));
        return Results.Ok(response);
    }
}