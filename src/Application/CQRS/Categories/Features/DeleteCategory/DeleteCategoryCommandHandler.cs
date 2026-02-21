namespace Application.CQRS.Categories.Features.DeleteCategory;

public sealed record DeleteCategoryCommandRequest(Guid Id) : IRequest<DeleteCategoryCommandResponse>;
public sealed record DeleteCategoryCommandResponse();

public sealed class DeleteCategoryCommandHandler(ICategoryService categoryService) : IRequestHandler<DeleteCategoryCommandRequest, DeleteCategoryCommandResponse>
{
    public async Task<DeleteCategoryCommandResponse> Handle(DeleteCategoryCommandRequest command, CancellationToken cancellationToken)
    {
        var deleted = categoryService.DeleteCategory(command.Id);

        if (!deleted)
            throw new InvalidOperationException("Category not found.");

        return new DeleteCategoryCommandResponse();
    }
}
