namespace Application.Features.Categories.Commands.CreateCategory;

public sealed class CreateCategoryCommandHandler(ICategoryService categoryService) : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest command, CancellationToken cancellationToken)
    {
        var category = categoryService.CreateCategory(command.CategoryRequest);
        return new CreateCategoryCommandResponse(category.Id);
    }
}

public sealed record CreateCategoryCommandRequest(CategoryRequest CategoryRequest) : IRequest<CreateCategoryCommandResponse>;
public sealed record CreateCategoryCommandResponse(Guid Id);
