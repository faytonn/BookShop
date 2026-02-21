namespace Application.CQRS.Books.Features.UpdateBook;

public sealed class UpdateBookEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/v1/books/{id:guid}", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!, Enum.GetName(UserRole.Seller)!));
    }

    private static async Task<IResult> Handler(ISender sender, Guid id, DTOs.BookRequest req)
    {
        var response = await sender.Send(new UpdateBookCommandRequest(id, req));
        return Results.Ok(response);
    }
}
