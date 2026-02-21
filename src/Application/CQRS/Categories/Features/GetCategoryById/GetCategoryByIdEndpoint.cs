namespace Application.CQRS.Categories.Features.GetCategoryById;

public sealed class GetCategoryByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/v1/categories/{id}", Handler);
    }

    private static async Task<IResult> Handler(string id, ISender sender)
    {
        var response = await sender.Send(new GetCategoryByIdQueryRequest(id));
        return Results.Ok(response);
    }
}
