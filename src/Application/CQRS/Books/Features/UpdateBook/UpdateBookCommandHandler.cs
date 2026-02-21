namespace Application.CQRS.Books.Features.UpdateBook;

public sealed record UpdateBookCommandRequest(Guid Id, DTOs.BookRequest BookRequest) : IRequest<UpdateBookCommandResponse>;
public sealed record UpdateBookCommandResponse(DTOs.BookResponse Book);

public sealed class UpdateBookCommandHandler(IBookService bookService) : IRequestHandler<UpdateBookCommandRequest, UpdateBookCommandResponse>
{
    public async Task<UpdateBookCommandResponse> Handle(UpdateBookCommandRequest command, CancellationToken cancellationToken)
    {
        var book = await bookService.UpdateBookAsync(command.Id, command.BookRequest);
        return new UpdateBookCommandResponse(book);
    }
}
