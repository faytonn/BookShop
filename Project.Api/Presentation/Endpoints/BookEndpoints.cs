namespace Project.Api.Presentation.Endpoints;

public static class BookEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapBookEndpoints()
        {
            var group = route.MapGroup("api/v1/books");

            group.MapGet("", async (IBookService bookService, IMemoryCache cache) =>
            {
                var books = await cache.GetOrCreateAsync("books:all", async entry =>
                {
                    entry.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
                    return await bookService.GetBooksAsync();
                });
                return Results.Ok(books);
            });


            group.MapGet("{id}", async (IBookService bookService, IMemoryCache cache, string id) =>
            {

                if (!Guid.TryParse(id, out var bookId))
                    return Results.BadRequest("Invalid Book Id.");

                var book = await cache.GetOrCreateAsync($"book:{bookId}", async entry =>
                {
                    entry.AbsoluteExpiration = DateTime.Now.AddMinutes(30);
                    return await bookService.GetBookAsync(bookId);
                });

                return Results.Ok(book);
            });


            group.MapPost("", async (IBookService bookService, BookRequest req, IHttpContextAccessor accessor) =>
            {
                var sellerId = accessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(sellerId) || !Guid.TryParse(sellerId, out var sellerGuid))
                    return Results.BadRequest("The requested user id not found.");

                try
                {
                    var bookId = await bookService.AddBookAsync(req, sellerGuid);
                    return Results.Created();
                }
                catch (DbUpdateException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (InvalidOperationException e)
                {
                    return Results.BadRequest(e.Message);
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            }).RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!, Enum.GetName(UserRole.Seller)!));


            group.MapPut("{id:guid}", async (IBookService bookService, Guid id, BookRequest req) =>
            {
                try
                {
                    var updated = await bookService.UpdateBookAsync(id, req);

                    if (!updated)
                        return Results.NotFound("Book not found.");

                    return Results.Ok("Book updated successfully.");
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
