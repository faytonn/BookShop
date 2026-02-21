namespace Application.CQRS.Categories.Features.CreateCategory;

public sealed record CreateCategoryCommandRequest(DTOs.CategoryRequest CategoryRequest) : IRequest<CreateCategoryCommandResponse>;
public sealed record CreateCategoryCommandResponse(Guid Id);

public sealed class CreateCategoryCommandHandler(ICategoryService categoryService) : IRequestHandler<CreateCategoryCommandRequest, CreateCategoryCommandResponse>
{
    public async Task<CreateCategoryCommandResponse> Handle(CreateCategoryCommandRequest command, CancellationToken cancellationToken)
    {
        var category = categoryService.CreateCategory(command.CategoryRequest);
        return new CreateCategoryCommandResponse(category.Id);
    }
}
