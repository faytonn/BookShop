namespace Application.CQRS.Categories.Features.UpdateCategory;

public sealed record UpdateCategoryCommandRequest(Guid Id, DTOs.CategoryRequest CategoryRequest) : IRequest<UpdateCategoryCommandResponse>;
public sealed record UpdateCategoryCommandResponse(DTOs.CategoryResponse Category);

public sealed class UpdateCategoryCommandHandler(ICategoryService categoryService) : IRequestHandler<UpdateCategoryCommandRequest, UpdateCategoryCommandResponse>
{
    public async Task<UpdateCategoryCommandResponse> Handle(UpdateCategoryCommandRequest command, CancellationToken cancellationToken)
    {
        var category = categoryService.UpdateCategory(command.Id, command.CategoryRequest);

        if (category is null)
            throw new InvalidOperationException("Category not found.");

        return new UpdateCategoryCommandResponse(category);
    }
}
