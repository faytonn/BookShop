using Domain.Models;
using System.Security.Claims;

namespace Application.CQRS.Books.Features.AddBook;

public sealed class AddBookEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/v1/books", Handler)
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!, Enum.GetName(UserRole.Seller)!));
    }

    private static async Task<IResult> Handler(ISender sender, DTOs.BookRequest req, IHttpContextAccessor accessor)
    {
        var sellerId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        var response = await sender.Send(new AddBookCommandRequest(req, sellerId));

        return Results.Created($"/api/v1/books/{response.BookId}", response);
    }
}
