namespace Application.CQRS.Categories.Features.GetCategoryBooks;

public sealed class GetCategoryBooksEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/categories/{id}/books", Handler);
    }

    private static async Task<IResult> Handler(string id, ISender sender)
    {
        var response = await sender.Send(new GetCategoryBooksQueryRequest(id));
        return Results.Ok(response);
    }
}
