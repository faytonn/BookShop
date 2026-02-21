namespace Application.Features.Categories.Queries.GetCategories;

public sealed class GetCategoriesQueryHandler(ICategoryService categoryService, IMemoryCache cache) : IRequestHandler<GetCategoriesQueryRequest, GetCategoriesQueryResponse>
{
    public async Task<GetCategoriesQueryResponse> Handle(GetCategoriesQueryRequest query, CancellationToken cancellationToken)
    {
        var categories = cache.GetOrCreate("categories:all", entry =>
        {
            entry.AbsoluteExpiration = DateTime.Now.AddHours(2);
            return categoryService.GetCategories().ToList();
        });

        return new GetCategoriesQueryResponse(categories);
    }
}

public sealed record GetCategoriesQueryRequest() : IRequest<GetCategoriesQueryResponse>;
public sealed record GetCategoriesQueryResponse(IEnumerable<CategoryResponse>? Data);
