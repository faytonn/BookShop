
namespace Project.Api.Features.Books.Commands.UpdateBook;

public sealed class UpdateBookCommandHandler(IBookService bookService) : IRequestHandler<UpdateBookCommandRequest, UpdateBookCommandResponse>
{
    public async Task<UpdateBookCommandResponse> Handle(UpdateBookCommandRequest command, CancellationToken cancellationToken)
    {
        var updated = await bookService.UpdateBookAsync(command.Request);

        return new UpdateBookCommandResponse(updated);
    }
}

public sealed record UpdateBookCommandRequest(UpdateBookRequest Request) : IRequest<UpdateBookCommandResponse>;

public sealed record UpdateBookCommandResponse(BookResponse Book);
