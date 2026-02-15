namespace Application.CQRS.Books.Features.GetBooks;

public sealed class GetBooksEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("books", Handler);
    }

    private static async Task<IResult> Handler(ISender sender)
    {
        var response = await sender.Send(new GetBooksQueryRequest());
        return Results.Ok(response);
    }
}