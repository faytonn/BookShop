using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Application.Services;
using Project.Api.Application.Services.Abstractions;
using System.Security.Claims;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/books"), ApiController]
public sealed class BooksController(IBookService bookService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetBooks()
    {
        var books = await bookService.GetBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetBook(string id)
    {
        if (!Guid.TryParse(id, out var bookId))
            return BadRequest("Invalid Book Id.");

        var book = await bookService.GetBookAsync(bookId);
        return Ok(book);
    }

    [HttpPost, Authorize(Roles = "Seller,Admin,SuperAdmin")]
    public async Task<IActionResult> AddBook(BookRequest req)
    {
        var sellerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(sellerId) || !Guid.TryParse(sellerId, out var sellerGuid))
            return BadRequest("The requested user id not found.");

        try
        {
            var bookId = await bookService.AddBookAsync(req, sellerGuid);
            return Created();
        }
        catch (DbUpdateException e)
        {
            return BadRequest(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:guid}"), Authorize(Roles = "Seller,Admin,SuperAdmin")]
    public async Task<IActionResult> UpdateBook(Guid id, BookRequest req)
    {
        try
        {
            var updated = await bookService.UpdateBookAsync(id, req);

            if (!updated)
                return NotFound("Book not found.");

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
        try
        {
            var deleted = await bookService.DeleteBookAsync(id);

            if (!deleted)
                return NotFound("Book not found.");

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