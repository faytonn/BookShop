using Project.Api.Application.DTOs;
using System.Text.Unicode;

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
                b.ReleaseDate,
                b.Authors.Select(a => new AuthorResponse
                (
                    a.AuthorId,
                    a.Author.Name
                )),
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
                b.Authors.Select(a => new AuthorResponse(
                    a.AuthorId,
                    a.Author.Name
                    )),
                b.Languages.Select(l => l.Language.Name)
            ))
            .FirstOrDefaultAsync();

        return book;
    }

    public async Task<Guid> AddBookAsync(BookRequest request, Guid sellerId)
    {
        var sellerExists = await unitOfWork.Sellers.FindAsync(sellerId);


        if (sellerExists == null)
            throw new InvalidDataException("You are not an authorized seller in this enterprise.");

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

            var authorNames = request.Authors.Select(a => a.Name);

            var authors = unitOfWork.Authors.GetWhereAll(a => authorNames
                                                    .Contains(a.Name)).ToList();
            
            if(authors.Count == 0)
            {
                foreach(var name in authorNames)
                {
                    var authorId = Guid.CreateVersion7();
                    unitOfWork.Authors.Add(new Author
                    {
                        Id = authorId,
                        Name = name
                    });

                    unitOfWork.BookAuthors.Add(new BookAuthor
                    {
                        AuthorId = authorId,
                        BookId = newBook.Id
                    });
                }
            }
            else
            {
                foreach (var name in authorNames)
                {
                    foreach (var author in authors)
                    {
                        if (string.Equals(name, author.Name, StringComparison.InvariantCultureIgnoreCase))
                        {
                            unitOfWork.BookAuthors.Add(new BookAuthor
                            {
                                AuthorId = author.Id,
                                BookId = newBook.Id
                            });
                        }
                        else
                        {
                            var authorId = Guid.CreateVersion7();
                            unitOfWork.Authors.Add(new Author
                            {
                                Id = authorId,
                                Name = name
                            });

                            unitOfWork.BookAuthors.Add(new BookAuthor
                            {
                                AuthorId = authorId,
                                BookId = newBook.Id
                            });
                        }
                    }
                }
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