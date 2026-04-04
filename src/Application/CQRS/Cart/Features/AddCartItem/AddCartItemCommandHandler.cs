namespace Application.CQRS.Cart.Features.AddCartItem;

public sealed record AddCartItemCommandRequest(Guid UserId, List<CartItemDto> Items) : IRequest<AddCartItemCommandResponse>;
public sealed record AddCartItemCommandResponse(IEnumerable<CartItemDto> Items);

public sealed class AddCartItemCommandHandler(AppDbContext context)
    : IRequestHandler<AddCartItemCommandRequest, AddCartItemCommandResponse>
{
    public async Task<AddCartItemCommandResponse> Handle(AddCartItemCommandRequest request, CancellationToken cancellationToken)
    {
        var books = await context.Books.AsNoTracking()
            .Where(b => request.Items.Select(i => i.BookId).Contains(b.Id))
            .ToArrayAsync(cancellationToken);

        foreach (var book in books)
        {
            if (!book.IsAvailable) throw new InvalidOperationException($"Book with ID {book.Id} is not available.");
        }

        var cartItems = request.Items.Select(item => new CartItem
        {
            Id = Guid.CreateVersion7(),
            UserId = request.UserId,
            BookId = item.BookId,
            Quantity = item.Quantity,
        });

        await context.CartItems.AddRangeAsync(cartItems, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        return new AddCartItemCommandResponse(cartItems.Select(ci => new CartItemDto(ci.Id, ci.BookId, ci.Quantity)));
    }
}