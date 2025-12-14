using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;
using Project.Api.Domain.Entities;
using Project.Api.Persistence.Contexts;

namespace Project.Api.Application.Services;

public sealed class CategoryService(AppDbContext context) : ICategoryService
{
    public IEnumerable<CategoryResponse> GetCategories()
    {
        var categories = context.Categories
            .Select(c => new CategoryResponse(
                c.Id,
                c.Name,
                c.PriorityLevel,
                c.ParentCategoryId
            ));

        return categories;
    }

    public CategoryResponse? GetCategory(Guid categoryId)
    {
        var category = context.Categories
            .Where(c => c.Id == categoryId)
            .Select(c => new CategoryResponse(
                c.Id,
                c.Name,
                c.PriorityLevel,
                c.ParentCategoryId
            ))
            .FirstOrDefault();

        return category;
    }

    public IEnumerable<Category> GetCategoryBooks(Guid categoryId)
    {
        var categories = context.Categories
            .Include(c => c.Books);

        return categories;
    }

    public CategoryResponse CreateCategory(CategoryRequest request)
    {
        if (request.ParentId != Guid.Empty)
        {
            var parentExists = context.Categories
                .Any(c => c.Id == request.ParentId);

            if (!parentExists)
                throw new InvalidOperationException("Parent category does not exist.");
        }

        var newCategory = new Category
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            PriorityLevel = request.PriorityLevel,
            ParentCategoryId = request.ParentId
        };

        context.Categories.Add(newCategory);
        context.SaveChanges();

        return new CategoryResponse(
            newCategory.Id,
            newCategory.Name,
            newCategory.PriorityLevel,
            newCategory.ParentCategoryId
        );
    }

    public CategoryResponse? UpdateCategory(Guid categoryId, CategoryRequest request)
    {
        var category = context.Categories
            .FirstOrDefault(c => c.Id == categoryId);

        if (category is null)
            return null;

        if (request.ParentId != Guid.Empty && request.ParentId != categoryId)
        {
            var parentExists = context.Categories
                .Any(c => c.Id == request.ParentId);

            if (!parentExists)
                throw new InvalidOperationException("Parent category does not exist.");

            if (request.ParentId == categoryId)
                throw new InvalidOperationException("Category cannot be its own parent.");
        }

        category.Name = request.Name;
        category.PriorityLevel = request.PriorityLevel;
        category.ParentCategoryId = request.ParentId;

        context.SaveChanges();

        return new CategoryResponse(
            category.Id,
            category.Name,
            category.PriorityLevel,
            category.ParentCategoryId
        );
    }

    public bool DeleteCategory(Guid categoryId)
    {
        var category = context.Categories
            .FirstOrDefault(c => c.Id == categoryId);

        if (category is null)
            return false;

        var hasSubcategories = context.Categories
            .Any(c => c.ParentCategoryId == categoryId);

        if (hasSubcategories)
            throw new InvalidOperationException("Cannot delete category with subcategories. Delete subcategories first.");

        var hasBooks = context.Books
            .Include(b => b.CategoryBooks)
            .Any(b => b.CategoryBooks.Any());

        if (hasBooks)
            throw new InvalidOperationException("Cannot delete category with associated books. Remove books first.");

        context.Categories.Remove(category);
        context.SaveChanges();

        return true;
    }
}