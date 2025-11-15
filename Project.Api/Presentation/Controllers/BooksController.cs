using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Domain.Entities;
using Project.Api.Persistence.Contexts;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/books"), ApiController]
public sealed class BooksController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public IActionResult GetBooks()
    {
        var sellers = new List<string>();
        sellers.AddRange("Kemale Guneshli", "Elza Seyidcahan");

        var books = context.Books.Select(b => new BookResponse(
                b.Id,
                b.Name,
                b.Price,
                decimal.Round(b.Price - (b.Price * b.Discount / 100), 2, MidpointRounding.ToPositiveInfinity),
                sellers
        ));

        return Ok(books);
    }

    [HttpGet("{id}")]
    public IActionResult GetBook(string id)
    {
        if (!Guid.TryParse(id, out var bookId)) return BadRequest("Invalid Book Id.");

        var sellers = new List<string>();
        sellers.AddRange("Kemale Guneshli", "Elza Seyidcahan");

        var book = context.Books
                 .Include(p => p.Languages)
                .Select(p => new BookDetailedResponse(
                p.Id,
                p.Name,
                p.Price,
                p.Discount,
                p.ReleaseDate,
                p.IsReleased,
                sellers,
                p.Languages.Select(l => l.Language.Name).ToList()
            )).FirstOrDefault();

        return Ok(book);
    }

    [HttpPost, Authorize(Roles = "Seller,Admin,SuperAdmin")]
    public IActionResult AddBook(BookRequest req)
    {
        var newBook = new Book
        {
            Id = Guid.CreateVersion7(),
            Name = req.Name,
            Price = req.Price,
            Discount = req.Discount,
            ReleaseDate = req.ReleaseDate,
        };

        using var transaction = context.Database.BeginTransaction();

        try
        {
            context.Books.Add(new Book
            {
                Id = newBook.Id,
                Name = newBook.Name,
                Price = newBook.Price,
                Discount = newBook.Discount,
                ReleaseDate = newBook.ReleaseDate,
            });

            context.SaveChanges();

            foreach (var langId in req.LanguageIds)
            {
                context.BooksLanguages.Add(new BookLanguage { 
                    BookId = newBook.Id,
                    LanguageId = langId,
                });
            }

            context.SaveChanges();

            transaction.Commit();

            return Created();

        }
        catch (DbUpdateException e)
        {
            transaction.Rollback();
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            transaction.Rollback();
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBook(Guid id)
    {
        var product = context.Books.Where(p => p.Id.Equals(id)).FirstOrDefault();
        if (product is null) return NotFound();

        try
        {
            context.Books.Remove(product);
            context.SaveChanges();

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