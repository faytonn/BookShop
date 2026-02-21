using Application.CQRS.Categories.DTOs;
using Domain.Models;

namespace Application.Services.Abstractions;

public interface ICategoryService
{
    IEnumerable<CategoryResponse> GetCategories();
    CategoryResponse? GetCategory(Guid categoryId);
    IEnumerable<Category> GetCategoryBooks(Guid categoryId);
    CategoryResponse CreateCategory(CategoryRequest request);
    CategoryResponse? UpdateCategory(Guid categoryId, CategoryRequest request);
    bool DeleteCategory(Guid categoryId);
}
