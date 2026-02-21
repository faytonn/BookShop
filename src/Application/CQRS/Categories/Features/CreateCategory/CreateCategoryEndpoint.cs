namespace Application.CQRS.Categories.Features.CreateCategory;

public sealed class CreateCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/categories", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, DTOs.CategoryRequest req)
    {
        var response = await sender.Send(new CreateCategoryCommandRequest(req));
        return Results.Created($"api/v1/categories/{response.Id}", response);
    }
}
