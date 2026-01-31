namespace Project.Api.Features.Books.Commands.AddBook;

public sealed class AddBookCommandHandler(IBookService bookService) : IRequestHandler<AddBookCommandRequest, AddBookCommandResponse>
{
    public async Task<AddBookCommandResponse> Handle(AddBookCommandRequest command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(command.SellerId) || !Guid.TryParse(command.SellerId, out var sellerId))
            throw new ArgumentException("The requested user id not found.");

        var book = command.BookRequest;

        var bookId = await bookService.AddBookAsync(book, sellerId);
        return new AddBookCommandResponse(bookId);
    }
}

public sealed record AddBookCommandRequest(BookRequest BookRequest, string? SellerId) : IRequest<AddBookCommandResponse>;
public sealed record AddBookCommandResponse(Guid BookId);