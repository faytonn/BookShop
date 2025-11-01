using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Domain.Entities;
using Project.Api.Persistence.Contexts;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/categories"), ApiController]
public sealed class CategoriesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCategories()
    {
        var categories = context.Categories
            .Select(c => new CategoryResponse(
                c.Id,
                c.Name,
                c.PriorityLevel,
                c.ParentCategoryId
            ));

        return Ok(categories);
    }

    [HttpGet("{id}")]
    public IActionResult GetCategory(string id)
    {
        if (!Guid.TryParse(id, out var categoryId))
            return BadRequest("Invalid Category Id.");

        var category = context.Categories
            .Where(c => c.Id == categoryId)
            .Select(c => new CategoryResponse(
                c.Id,
                c.Name,
                c.PriorityLevel,
                c.ParentCategoryId
            ))
            .FirstOrDefault();

        if (category is null)
            return NotFound("Category not found.");

        return Ok(category);
    }

    public IActionResult GetCategoryBooks(string id)
    {
        if (!Guid.TryParse(id, out var categoryId))
            return BadRequest("Invalid Category Id.");

        var categories = context.Categories
            .Include(c => c.Books);

        if (categories is null)
            return NotFound("Category not found.");

        return Ok(categories);
    }


    //[HttpGet("{id}/subcategories")]
    //public IActionResult GetSubcategories(string id)
    //{
    //    if (!Guid.TryParse(id, out var categoryId))
    //        return BadRequest("Invalid Category Id.");

    //    var subcategories = context.Categories
    //        .Where(c => c.ParentCategoryId == categoryId)
    //        .Select(c => new CategoryResponse(
    //            c.Id,
    //            c.Name,
    //            c.PriorityLevel,
    //            c.ParentCategoryId
    //        ))
    //        .ToList();

    //    return Ok(subcategories);
    //}

    [HttpPost, Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult CreateCategory(CategoryRequest req)
    {
        if (req.ParentId != Guid.Empty)
        {
            var parentExists = context.Categories
                .Any(c => c.Id == req.ParentId);

            if (!parentExists)
                return BadRequest("Parent category does not exist.");
        }

        var newCategory = new Category
        {
            Id = Guid.CreateVersion7(),
            Name = req.Name,
            PriorityLevel = req.PriorityLevel,
            ParentCategoryId = req.ParentId
        };

        try
        {
            context.Categories.Add(newCategory);
            context.SaveChanges();

            var response = new CategoryResponse(
                newCategory.Id,
                newCategory.Name,
                newCategory.PriorityLevel,
                newCategory.ParentCategoryId
            );

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = newCategory.Id },
                response
            );
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:guid}"), Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult UpdateCategory(Guid id, CategoryRequest req)
    {
        var category = context.Categories
            .FirstOrDefault(c => c.Id == id);

        if (category is null)
            return NotFound("Category not found.");

        if (req.ParentId != Guid.Empty && req.ParentId != id)
        {
            var parentExists = context.Categories
                .Any(c => c.Id == req.ParentId);

            if (!parentExists)
                return BadRequest("Parent category does not exist.");

            if (req.ParentId == id)
                return BadRequest("Category cannot be its own parent.");
        }

        try
        {
            category.Name = req.Name;
            category.PriorityLevel = req.PriorityLevel;
            category.ParentCategoryId = req.ParentId;

            context.SaveChanges();

            var response = new CategoryResponse(
                category.Id,
                category.Name,
                category.PriorityLevel,
                category.ParentCategoryId
            );

            return Ok(response);
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }


    [HttpDelete("{id:guid}"), Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult DeleteCategory(Guid id)
    {
        var category = context.Categories
            .FirstOrDefault(c => c.Id == id);

        if (category is null)
            return NotFound("Category not found.");

        var hasSubcategories = context.Categories
            .Any(c => c.ParentCategoryId == id);

        if (hasSubcategories)
            return BadRequest("Cannot delete category with subcategories. Delete subcategories first.");

        var hasBooks = context.Books
            .Include(b => b.CategoryBooks)
            .Any(b => b.CategoryBooks.Any());

        if (hasBooks)
            return BadRequest("Cannot delete category with associated books. Remove books first.");

        try
        {
            context.Categories.Remove(category);
            context.SaveChanges();
            return NoContent();
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
