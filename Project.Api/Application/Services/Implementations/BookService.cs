using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;
using Project.Api.Persistence.Repositories.BookLanguages;
using Project.Api.Persistence.Repositories.Books;
using Project.Api.Persistence.Repositories.BookSellers;

namespace Project.Api.Application.Services;

public sealed class BookService(IBookRepository bookRepository,
                                IBookLanguageRepository bookLanguageRepository, 
                                IBookSellerRepository bookSellerRepository) : IBookService
{
    public async Task<IEnumerable<BookResponse>> GetBooksAsync()
    {
        var books = await bookRepository
            .GetBooksWithSellers()
            .Select(b => new BookResponse(
                b.Id,
                b.Name,
                decimal.Round(b.Price - (b.Price * b.Discount / 100), 2, MidpointRounding.ToPositiveInfinity),
                b.Discount,
                b.BookSellers.Select(bs => bs.Seller.Name)
            ))
            .ToListAsync();
        return books;
    }

    public async Task<BookDetailedResponse?> GetBookAsync(Guid bookId)
    {
        var book = await bookRepository
            .GetBooksWithLanguagesThenSellers()
            .Where(b => b.Id == bookId)
            .Select(b => new BookDetailedResponse(
                b.Id,
                b.Name,
                b.Price,
                b.Discount,
                b.ReleaseDate,
                b.IsReleased,
                b.BookSellers.Select(bs => bs.Seller.Name),
                b.Languages.Select(l => l.Language.Name)
            ))
            .FirstOrDefaultAsync();

        return book;
    }

    public async Task<Guid> AddBookAsync(BookRequest request, Guid sellerId)
    {
        var newBook = new Book
        {
            Id = Guid.CreateVersion7(),
            Name = request.Name,
            Price = request.Price,
            Discount = request.Discount,
            ReleaseDate = request.ReleaseDate,
        };

        using var transaction = await bookRepository.BeginTransactionAsync();

        try
        {
            bookRepository.Add(newBook);

            foreach (var langId in request.LanguageIds)
            {
                bookLanguageRepository.Add(new BookLanguage
                {
                    BookId = newBook.Id,
                    LanguageId = langId,
                });
            }

            var bookSeller = new BookSeller
            {
                BookId = newBook.Id,
                SellerId = sellerId
            };

            bookSellerRepository.Add(bookSeller);

            await bookRepository.SaveChangesAsync();
            await transaction.CommitAsync();

            return newBook.Id;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> UpdateBookAsync(Guid bookId, BookRequest request)
    {
        var book = await bookRepository.FindAsync(bookId);
        if (book is null)
            return false;

        book.Name = request.Name;
        book.Price = request.Price;
        book.Discount = request.Discount;
        book.ReleaseDate = request.ReleaseDate;

        await bookRepository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteBookAsync(Guid bookId)
    {
        var book = await bookRepository.FindAsync(bookId);
        if (book is null)
            return false;

        bookRepository.Remove(book);
        await bookRepository.SaveChangesAsync();

        return true;
    }
}