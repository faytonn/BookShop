using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;
using Project.Api.Persistence.UnitOfWorks;

namespace Project.Api.Application.Services;

public sealed class BookService(IUnitOfWork unitOfWork) : IBookService
{
    public async Task<IEnumerable<BookResponse>> GetBooksAsync()
    {
        var books = await unitOfWork.Books
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
        var book = await unitOfWork.Books
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

        using var transaction = await unitOfWork.BeginTransactionAsync();

        try
        {
            unitOfWork.Books.Add(newBook);

            foreach (var langId in request.LanguageIds)
            {
                unitOfWork.BookLanguages.Add(new BookLanguage
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

            unitOfWork.BookSellers.Add(bookSeller);

            await unitOfWork.SaveChangesAsync();
            await unitOfWork.CommitAsync();

            return newBook.Id;
        }
        catch
        {
            await unitOfWork.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> UpdateBookAsync(Guid bookId, BookRequest request)
    {
        var book = await unitOfWork.Books.FindAsync(bookId);
        if (book is null)
            return false;

        book.Name = request.Name;
        book.Price = request.Price;
        book.Discount = request.Discount;
        book.ReleaseDate = request.ReleaseDate;

        await unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteBookAsync(Guid bookId)
    {
        var book = await unitOfWork.Books.FindAsync(bookId);
        if (book is null)
            return false;

        unitOfWork.Books.Remove(book);
        await unitOfWork.SaveChangesAsync();

        return true;
    }
}