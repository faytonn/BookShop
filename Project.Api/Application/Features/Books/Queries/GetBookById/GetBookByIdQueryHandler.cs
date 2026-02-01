namespace Project.Api.Application.Features.Books.Queries.GetBookById;

public sealed class GetBookByIdQueryHandler(IBookService bookService, IMemoryCache cache) : IRequestHandler<GetBookByIdQueryRequest, GetBookByIdQueryResponse>
{
    public async Task<GetBookByIdQueryResponse> Handle(GetBookByIdQueryRequest query, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(query.BookId, out var bookId))
            throw new ArgumentException("Invalid Book Id.");

        var book = await cache.GetOrCreateAsync($"book:{bookId}", async entry =>
        {
            entry.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            return await bookService.GetBookAsync(bookId);
        });

        return new GetBookByIdQueryResponse(book);
    }
}

public sealed record GetBookByIdQueryRequest(string BookId) : IRequest<GetBookByIdQueryResponse>;
public sealed record GetBookByIdQueryResponse(BookDetailedResponse? Book);