using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/categories"), ApiController]
public sealed class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    public IActionResult GetCategories()
    {
        var categories = categoryService.GetCategories();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public IActionResult GetCategory(string id)
    {
        if (!Guid.TryParse(id, out var categoryId))
            return BadRequest("Invalid Category Id.");

        var category = categoryService.GetCategory(categoryId);

        if (category is null)
            return NotFound("Category not found.");

        return Ok(category);
    }

    [HttpGet("{id}/books")]
    public IActionResult GetCategoryBooks(string id)
    {
        if (!Guid.TryParse(id, out var categoryId))
            return BadRequest("Invalid Category Id.");

        var categories = categoryService.GetCategoryBooks(categoryId);

        if (categories is null)
            return NotFound("Category not found.");

        return Ok(categories);
    }

    [HttpPost, Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult CreateCategory(CategoryRequest req)
    {
        try
        {
            var response = categoryService.CreateCategory(req);

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = response.Id },
                response
            );
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
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
        try
        {
            var response = categoryService.UpdateCategory(id, req);

            if (response is null)
                return NotFound("Category not found.");

            return Ok(response);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
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
        try
        {
            var deleted = categoryService.DeleteCategory(id);

            if (!deleted)
                return NotFound("Category not found.");

            return NoContent();
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
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