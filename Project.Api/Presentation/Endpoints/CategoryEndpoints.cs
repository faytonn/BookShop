using Project.Api.Application.Services.Implementations;
using System.Runtime.InteropServices.ComTypes;

namespace Project.Api.Presentation.Endpoints;

public static class CategoryEndpoints
{
    extension(IEndpointRouteBuilder route)
    {
        public void MapCategoryEndpoints()
        {
            var group = route.MapGroup("api/v1/categories");

            group.MapGet("", async (ICategoryService categoryService, IMemoryCache cache) =>
            {
                var categories = cache.GetOrCreate("categories:all", entry =>
                {
                    entry.AbsoluteExpiration = DateTime.Now.AddHours(2);
                    return categoryService.GetCategories().ToList();
                });

                return Results.Ok(categories);
            });


            group.MapGet("{id}", async (ICategoryService categoryService, IMemoryCache cache, string id) =>
            {
                if (!Guid.TryParse(id, out var categoryId))
                    return Results.BadRequest("Invalid Category Id.");

                var category = cache.GetOrCreate($"category:{categoryId}", entry =>
                {
                    entry.AbsoluteExpiration = DateTime.Now.AddHours(2);
                    return categoryService.GetCategory(categoryId);
                });

                if (category is null)
                    return Results.NotFound("Category not found.");

                return Results.Ok(category);

            });


            group.MapGet("{id}/books", async (ICategoryService categoryService, string id) =>
            {
                if (!Guid.TryParse(id, out var categoryId))
                    return Results.BadRequest("Invalid Category Id.");

                var categories = categoryService.GetCategoryBooks(categoryId);

                if (categories is null)
                    return Results.NotFound("Category not found.");

                return Results.Ok(categories);
            });

            group.MapPost("", async (ICategoryService categoryService, CategoryRequest req) =>
            {
                try
                {
                    var response = categoryService.CreateCategory(req);

                    return Results.Created(
                        $"api/v1/categories/{response.Id}",
                        new { id = response.Id }
                    );
                }
                catch (InvalidOperationException e)
                {
                    return Results.BadRequest(e.Message);
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


            group.MapPut("{id:guid}", async (ICategoryService categoryService, Guid id, CategoryRequest req) =>
            {
                try
                {
                    var response = categoryService.UpdateCategory(id, req);

                    if (response is null)
                        return Results.NotFound("Category not found.");

                    return Results.Ok(response);
                }
                catch (InvalidOperationException e)
                {
                    return Results.BadRequest(e.Message);
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


            group.MapDelete("{id:guid}", async (ICategoryService categoryService, Guid id) => 
            {
                try
                {
                    var deleted = categoryService.DeleteCategory(id);

                    if (!deleted)
                        return Results.NotFound("Category not found.");

                    return Results.NoContent();
                }
                catch (InvalidOperationException e)
                {
                    return Results.BadRequest(e.Message);
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
