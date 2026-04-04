namespace Application.CQRS.Cart.Features.AddCartItem;

public sealed record AddCartItemCommandRequest(Guid UserId, List<AddCartItemDto> Items) : IRequest<AddCartItemCommandResponse>;
public sealed record AddCartItemCommandResponse(IEnumerable<CartItemDto> Items);

public sealed class AddCartItemCommandHandler(AppDbContext context)
    : IRequestHandler<AddCartItemCommandRequest, AddCartItemCommandResponse>
{
    public async Task<AddCartItemCommandResponse> Handle(AddCartItemCommandRequest request, CancellationToken cancellationToken)
    {
        var cart = await context.Carts
                .Include(c => c.Items)
                .Where(c => c.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? throw new InvalidOperationException("No cart found for this user.");

        var books = await context.Books.AsNoTracking()
            .Where(b => request.Items.Select(i => i.BookId).Contains(b.Id))
            .ToArrayAsync(cancellationToken);

        foreach (var book in books)
        {
            if (!book.IsAvailable) throw new InvalidOperationException($"Book with ID {book.Id} is not available.");
        }

        foreach (var cartItem in cart.Items)
        {
            var existingItem = request.Items.FirstOrDefault(item => item.BookId == cartItem.BookId);
            if (existingItem is not null)
            {
                cartItem.Quantity += 1; // Client-dan gelen data icine third person mudaxile etmesini istemediyimiz ucun quantity statik qoyuruq
                request.Items.Remove(existingItem);
            }
        }

        var cartItems = request.Items.Select(item => new CartItem
        {
            Id = Guid.CreateVersion7(),
            CartId = cart.Id,
            UserId = request.UserId,
            BookId = item.BookId,
            Quantity = item.Quantity,
        });

        cart.Items.AddRange(cartItems);
        await context.SaveChangesAsync(cancellationToken);

        return new AddCartItemCommandResponse(cartItems.Select(ci => new CartItemDto(ci.Id, ci.BookId, ci.Quantity)));
    }
}