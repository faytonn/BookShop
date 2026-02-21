namespace Application.CQRS.Categories.Features.DeleteCategory;

public sealed class DeleteCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/categories/{id:guid}", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id)
    {
        await sender.Send(new DeleteCategoryCommandRequest(id));
        return Results.NoContent();
    }
}
