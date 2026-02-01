namespace Project.Api.Features.Books.Queries.GetBooks;

public sealed class GetBooksQueryHandler(IBookService bookService, IMemoryCache cache) : IRequestHandler<GetBooksQueryRequest, GetBooksQueryResponse>
{
    public async Task<GetBooksQueryResponse> Handle(GetBooksQueryRequest query, CancellationToken cancellationToken)
    {
        var books = await cache.GetOrCreateAsync("books:all", async entry =>
        {
            entry.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
            return await bookService.GetBooksAsync();
        });

        return new GetBooksQueryResponse(books);
    }
}

public sealed record GetBooksQueryRequest() : IRequest<GetBooksQueryResponse>;

public sealed record GetBooksQueryResponse(IEnumerable<BookResponse>? Data);