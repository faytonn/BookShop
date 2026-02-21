using Application.CQRS.Books.DTOs;

namespace Application.Services.Abstractions
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponse>> GetBooksAsync();
        Task<BookDetailedResponse?> GetBookAsync(Guid bookId);
        Task<Guid> AddBookAsync(BookRequest request, Guid sellerId);
        Task<BookResponse> UpdateBookAsync(Guid bookId, BookRequest request);
        Task<bool> DeleteBookAsync(Guid bookId);
    }
}
