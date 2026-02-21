namespace Application.CQRS.Books.Features.GetBookById;

public sealed class GetBookByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/books/{id}", Handler);
    }

    private static async Task<IResult> Handler(string id, ISender sender)
    {
        var response = await sender.Send(new GetBookByIdQueryRequest(id));
        return Results.Ok(response);
    }
}
