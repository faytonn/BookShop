namespace Project.Api.Application.Services.Abstractions
{
    public interface IBookService
    {
        Task<IEnumerable<BookResponse>> GetBooksAsync();
        Task<BookDetailedResponse?> GetBookAsync(Guid bookId);
        Task<Guid> AddBookAsync(BookRequest request, Guid sellerId);
        Task<bool> UpdateBookAsync(Guid bookId, BookRequest request);
        Task<bool> DeleteBookAsync(Guid bookId);
    }
}
