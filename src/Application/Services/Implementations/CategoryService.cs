namespace Application.Services.Implementations;

public sealed class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
{
    public IEnumerable<CategoryResponse> GetCategories()
    {
        var categories = unitOfWork.Categories
            .GetAll(tracking: false)
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
        var category = unitOfWork.Categories
            .GetWhereAll(c => c.Id == categoryId)
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
        var categories = unitOfWork.Categories
            .GetAll(tracking: false)
            .Include(c => c.Books);

        return categories;
    }

    public CategoryResponse CreateCategory(CategoryRequest request)
    {
        if (request.ParentId != Guid.Empty)
        {
            var parentExists = unitOfWork.Categories
                .GetWhereAll(c => c.Id == request.ParentId)
                .Any();

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

        unitOfWork.Categories.Add(newCategory);
        unitOfWork.SaveChanges();

        return new CategoryResponse(
            newCategory.Id,
            newCategory.Name,
            newCategory.PriorityLevel,
            newCategory.ParentCategoryId
        );
    }

    public CategoryResponse? UpdateCategory(Guid categoryId, CategoryRequest request)
    {
        var category = unitOfWork.Categories.Find(categoryId);

        if (category is null)
            return null;

        if (request.ParentId != Guid.Empty && request.ParentId != categoryId)
        {
            var parentExists = unitOfWork.Categories
                .GetWhereAll(c => c.Id == request.ParentId)
                .Any();

            if (!parentExists)
                throw new InvalidOperationException("Parent category does not exist.");

            if (request.ParentId == categoryId)
                throw new InvalidOperationException("Category cannot be its own parent.");
        }

        category.Name = request.Name;
        category.PriorityLevel = request.PriorityLevel;
        category.ParentCategoryId = request.ParentId;


        unitOfWork.Categories.Update(category);
        unitOfWork.SaveChanges();

        return new CategoryResponse(
            category.Id,
            category.Name,
            category.PriorityLevel,
            category.ParentCategoryId
        );
    }

    public bool DeleteCategory(Guid categoryId)
    {
        var category = unitOfWork.Categories
            .Find(categoryId);

        if (category is null)
            return false;

        var hasSubcategories = unitOfWork.Categories
            .GetWhereAll(c => c.ParentCategoryId == categoryId)
            .Any();

        if (hasSubcategories)
            throw new InvalidOperationException("Cannot delete category with subcategories. Delete subcategories first.");

        var hasBooks = unitOfWork.Books
            .GetBooksWithCategories()
            .Any(b => b.CategoryBooks.Any(cb => cb.CategoryId == categoryId));

        if (hasBooks)
            throw new InvalidOperationException("Cannot delete category with associated books. Remove books first.");

        unitOfWork.Categories.Remove(category);
        unitOfWork.SaveChanges();

        return true;
    }
}
