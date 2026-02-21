using Application.Services.Abstractions;
using Microsoft.Extensions.Caching.Memory;

namespace Application.CQRS.Categories.Features.GetCategoryById;

public sealed record GetCategoryByIdQueryRequest(string CategoryId) : IRequest<GetCategoryByIdQueryResponse>;
public sealed record GetCategoryByIdQueryResponse(DTOs.CategoryResponse Category);

public sealed class GetCategoryByIdQueryHandler(ICategoryService categoryService, IMemoryCache cache) : IRequestHandler<GetCategoryByIdQueryRequest, GetCategoryByIdQueryResponse>
{
    public async Task<GetCategoryByIdQueryResponse> Handle(GetCategoryByIdQueryRequest query, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(query.CategoryId, out var categoryId))
            throw new ArgumentException("Invalid Category Id.");

        var category = cache.GetOrCreate($"category:{categoryId}", entry =>
        {
            entry.AbsoluteExpiration = DateTime.Now.AddHours(2);
            return categoryService.GetCategory(categoryId);
        });

        if (category is null)
            throw new InvalidOperationException("Category not found.");

        return new GetCategoryByIdQueryResponse(category);
    }
}
