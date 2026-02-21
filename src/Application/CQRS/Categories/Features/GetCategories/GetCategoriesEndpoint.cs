namespace Application.CQRS.Categories.Features.GetCategories;

public sealed class GetCategoriesEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/categories", Handler);
    }

    private static async Task<IResult> Handler(ISender sender)
    {
        var response = await sender.Send(new GetCategoriesQueryRequest());
        return Results.Ok(response);
    }
}
