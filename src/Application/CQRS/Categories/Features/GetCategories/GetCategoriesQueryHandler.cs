namespace Application.CQRS.Categories.Features.GetCategories;

public sealed record GetCategoriesQueryRequest() : IRequest<GetCategoriesQueryResponse>;
public sealed record GetCategoriesQueryResponse(IEnumerable<DTOs.CategoryResponse>? Data);

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
