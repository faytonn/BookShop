namespace Application.Features.Categories.Commands.UpdateCategory;

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

public sealed record UpdateCategoryCommandRequest(Guid Id, CategoryRequest CategoryRequest) : IRequest<UpdateCategoryCommandResponse>;
public sealed record UpdateCategoryCommandResponse(CategoryResponse Category);
