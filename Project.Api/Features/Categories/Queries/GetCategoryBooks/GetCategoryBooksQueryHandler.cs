namespace Project.Api.Features.Categories.Queries.GetCategoryBooks;

public sealed class GetCategoryBooksQueryHandler(ICategoryService categoryService) : IRequestHandler<GetCategoryBooksQueryRequest, GetCategoryBooksQueryResponse>
{
    public async Task<GetCategoryBooksQueryResponse> Handle(GetCategoryBooksQueryRequest query, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(query.CategoryId, out var categoryId))
            throw new ArgumentException("Invalid Category Id.");

        var books = categoryService.GetCategoryBooks(categoryId);

        if (books is null)
            throw new InvalidOperationException("Category not found.");

        return new GetCategoryBooksQueryResponse(books);
    }
}

public sealed record GetCategoryBooksQueryRequest(string CategoryId) : IRequest<GetCategoryBooksQueryResponse>;
public sealed record GetCategoryBooksQueryResponse(IEnumerable<Category>? Data);