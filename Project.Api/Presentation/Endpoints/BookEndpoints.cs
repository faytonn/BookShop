

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


            group.MapPut("{id:guid}", async (ISender sender, Guid id, BookRequest req) =>
            {
                var response = await sender.Send(new UpdateBookCommandRequest(id, req));
                return Results.Ok(response);
            })
             .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!, Enum.GetName(UserRole.Seller)!));


            group.MapDelete("{id:guid}", async (ISender sender, Guid id) =>
            {
                await sender.Send(new DeleteBookCommandRequest(id));
                return Results.NoContent();
            }).RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));


        }
    }
}
