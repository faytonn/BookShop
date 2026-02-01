

namespace Project.Api.Presentation.Endpoints;

public static class CategoryEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapCategoryEndpoints()
        {
            var group = route.MapGroup("api/v1/categories");

            group.MapGet("", async (ISender sender) =>
            {
                var response = await sender.Send(new GetCategoriesQueryRequest());

                return Results.Ok(response);
            });

            group.MapGet("{id}", async (string id, ISender sender) =>
            {
                var response = await sender.Send(new GetCategoryByIdQueryRequest(id));

                return Results.Ok(response);
            });

            group.MapGet("{id}/books", async (string id, ISender sender) =>
            {
                var response = await sender.Send(new GetCategoryBooksQueryRequest(id));

                return Results.Ok(response);
            });

            group.MapPost("", async (ISender sender, CategoryRequest req) =>
            {
                var response = await sender.Send(new CreateCategoryCommandRequest(req));

                return Results.Created($"api/v1/categories/{response.Id}", response);
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));

            group.MapPut("{id:guid}", async (ISender sender, Guid id, CategoryRequest req) =>
            {
                var response = await sender.Send(new UpdateCategoryCommandRequest(id, req));

                return Results.Ok(response);
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));

            group.MapDelete("{id:guid}", async (ISender sender, Guid id) =>
            {
                await sender.Send(new DeleteCategoryCommandRequest(id));

                return Results.NoContent();
            })
            .RequireAuthorization(policy => policy.RequireRole(Enum.GetName(UserRole.Admin)!, Enum.GetName(UserRole.SuperAdmin)!));

        }
    }
}