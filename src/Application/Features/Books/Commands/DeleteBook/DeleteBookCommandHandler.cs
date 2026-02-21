namespace Application.Features.Books.Commands.DeleteBook;

public sealed class DeleteBookCommandHandler(IBookService bookService) : IRequestHandler<DeleteBookCommandRequest, DeleteBookCommandResponse>
{
    public async Task<DeleteBookCommandResponse> Handle(DeleteBookCommandRequest command, CancellationToken cancellationToken)
    {
        var deleted = await bookService.DeleteBookAsync(command.Id);

        if (!deleted)
            throw new InvalidOperationException("Book not found.");

        return new DeleteBookCommandResponse();
    }
}

public sealed record DeleteBookCommandRequest(Guid Id) : IRequest<DeleteBookCommandResponse>;
public sealed record DeleteBookCommandResponse();
