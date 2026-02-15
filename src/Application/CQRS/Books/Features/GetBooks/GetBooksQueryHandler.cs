namespace Application.CQRS.Books.Features.GetBooks;

public record GetBooksQueryRequest() : IRequest<GetBooksQueryResponse>;
public record GetBooksQueryResponse(BookDto Book);


public sealed class GetBooksQueryHandler(AppDbContext context) : IRequestHandler<GetBooksQueryRequest, GetBooksQueryResponse>
{
    public Task<GetBooksQueryResponse> Handle(GetBooksQueryRequest request, CancellationToken cancellationToken)
    {
        return default;
    }
}