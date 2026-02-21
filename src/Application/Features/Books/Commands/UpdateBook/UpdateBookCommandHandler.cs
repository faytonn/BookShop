namespace Application.Features.Books.Commands.UpdateBook;

public sealed class UpdateBookCommandHandler(IBookService bookService) : IRequestHandler<UpdateBookCommandRequest, UpdateBookCommandResponse>
{
    public async Task<UpdateBookCommandResponse> Handle(UpdateBookCommandRequest command, CancellationToken cancellationToken)
    {
        var book = await bookService.UpdateBookAsync(command.Id, command.BookRequest);
        return new UpdateBookCommandResponse(book);
    }
}

public sealed record UpdateBookCommandRequest(Guid Id, BookRequest BookRequest) : IRequest<UpdateBookCommandResponse>;
public sealed record UpdateBookCommandResponse(BookResponse Book);
