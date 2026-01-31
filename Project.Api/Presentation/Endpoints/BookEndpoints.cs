using Project.Api.Features.Books.Commands.AddBook;
using Project.Api.Features.Books.Queries.GetBookById;
using Project.Api.Features.Books.Queries.GetBooks;

namespace Project.Api.Presentation.Endpoints;

public static class BookEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapBookEndpoints()
        {
            var group = route.MapGroup("api/v1/books");

            group.MapGet("", async (ISender sender) =>
            {
                var response = await sender.Send(new GetBooksQueryRequest());
                return Results.Ok(response);
            });

            group.MapGet("{id}", async (string id, ISender sender) =>
            {
                var response = await sender.Send(new GetBookByIdQueryRequest(id));
                return Results.Ok(response);
            });


            group.MapPost("", async (ISender sender, BookRequest req, IHttpContextAccessor accessor) =>
            {
                var sellerId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                var response = await sender.Send(new AddBookCommandRequest(req, sellerId));

                return Results.Created($"/api/v1/books/{response.BookId}", response);
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!, Enum.GetName(UserRole.Seller)!));


            group.MapPut("", async (ISender sender, IBookService bookService, UpdateBookRequest req) =>
            {
                try
                {
                    //var updated = await bookService.UpdateBookAsync(req);

                    //if (!updated)
                    //    return Results.NotFound("Book not found.");

                    //return Results.Ok("Book updated successfully.");
                }
                catch (DbUpdateException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }).RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!, Enum.GetName(UserRole.Seller)!));


            group.MapDelete("{id:guid}", async (IBookService bookService, Guid id) =>
            {
                try
                {
                    var deleted = await bookService.DeleteBookAsync(id);

                    if (!deleted)
                        return Results.NotFound("Book not found.");

                    return Results.NoContent();
                }
                catch (DbUpdateException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }).RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));

        }
    }
}
