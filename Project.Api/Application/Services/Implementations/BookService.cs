using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;
using Project.Api.Persistence.Contexts;

namespace Project.Api.Application.Services;

public sealed class BookService(AppDbContext context) : IBookService
{
    public async Task<IEnumerable<BookResponse>> GetBooksAsync()
    {
        var books = await context.Books
            .Include(b => b.BookSellers)
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
        var book = await context.Books
            .Include(b => b.Languages)
            .Include(b => b.BookSellers)
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

        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            context.Books.Add(newBook);

            foreach (var langId in request.LanguageIds)
            {
                context.BooksLanguages.Add(new BookLanguage
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

            context.BookSellers.Add(bookSeller);

            await context.SaveChangesAsync();
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
        var book = await context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        if (book is null)
            return false;

        book.Name = request.Name;
        book.Price = request.Price;
        book.Discount = request.Discount;
        book.ReleaseDate = request.ReleaseDate;

        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteBookAsync(Guid bookId)
    {
        var book = await context.Books.FirstOrDefaultAsync(b => b.Id == bookId);
        if (book is null)
            return false;

        context.Books.Remove(book);
        await context.SaveChangesAsync();

        return true;
    }
}