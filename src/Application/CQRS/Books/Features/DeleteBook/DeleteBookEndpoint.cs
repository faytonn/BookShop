namespace Application.CQRS.Books.Features.DeleteBook;

public sealed class DeleteBookEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/v1/books/{id:guid}", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id)
    {
        await sender.Send(new DeleteBookCommandRequest(id));
        return Results.NoContent();
    }
}
