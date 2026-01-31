namespace Project.Api.Features.Books.Queries.GetBooks;

public sealed record GetBooksQueryResponse(IEnumerable<BookResponse>? Data);