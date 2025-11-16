using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Domain.Entities;
using Project.Api.Persistence.Contexts;
using System.Security.Claims;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/books"), ApiController]
public sealed class BooksController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBooks()
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

        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(string id)
    {
        if (!Guid.TryParse(id, out var bookId))
            return BadRequest("Invalid Book Id.");

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

        if (book is null)
            return NotFound("Book not found.");

        return Ok(book);
    }

    [HttpPost, Authorize(Roles = "Seller,Admin,SuperAdmin")]
    public async Task<IActionResult> AddBook(BookRequest req)
    {
        var newBook = new Book
        {
            Id = Guid.CreateVersion7(),
            Name = req.Name,
            Price = req.Price,
            Discount = req.Discount,
            ReleaseDate = req.ReleaseDate,
        };

        using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            context.Books.Add(newBook);
            foreach (var langId in req.LanguageIds)
            {
                context.BooksLanguages.Add(new BookLanguage
                {
                    BookId = newBook.Id,
                    LanguageId = langId,
                });
            }

            var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new InvalidOperationException("The requested user id not found.");

            var bookSeller = new BookSeller
            {
                BookId = newBook.Id,
                SellerId = Guid.Parse(sellerId)
            };

            context.BookSellers.Add(bookSeller);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Created();
        }
        catch (DbUpdateException e)
        {
            await transaction.RollbackAsync();
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            await transaction.RollbackAsync();
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:guid}"), Authorize(Roles = "Seller,Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateBook(Guid id, BookRequest req)
    {
        var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book is null)
            return NotFound("Book not found.");

        try
        {
            book.Name = req.Name;
            book.Price = req.Price;
            book.Discount = req.Discount;
            book.ReleaseDate = req.ReleaseDate;

            await context.SaveChangesAsync();

            return Ok("Book updated successfully.");
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:guid}"), Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var book = await context.Books.FirstOrDefaultAsync(b => b.Id == id);
        if (book is null)
            return NotFound("Book not found.");

        try
        {
            context.Books.Remove(book);
            await context.SaveChangesAsync();

            return NoContent();
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}