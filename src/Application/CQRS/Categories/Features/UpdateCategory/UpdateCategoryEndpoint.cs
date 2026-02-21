namespace Application.CQRS.Categories.Features.UpdateCategory;

public sealed class UpdateCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/categories/{id:guid}", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id, DTOs.CategoryRequest req)
    {
        var response = await sender.Send(new UpdateCategoryCommandRequest(id, req));
        return Results.Ok(response);
    }
}
