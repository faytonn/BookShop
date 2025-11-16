using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Domain.Entities;
using Project.Api.Persistence.Contexts;
using System.Text.Json;
using System.Security.Claims;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/orders"), ApiController]

public sealed class OrdersController(AppDbContext context) : ControllerBase
{
    private static string GenerateDisplayCode(Guid id) => $"ORDER{id.ToString("N")[..8].ToUpper()}";

    // ORDER6754GBGSF-5656-34343

    [HttpPost, Authorize]
    public async Task<IActionResult> AddOrder(AddOrderRequest request)
    {
        //OrderItemDTO

        // check ele, ele bir user var mi
        // tokenle olsun
        // try catch, databazaya yazilsin
        //2 alqoritma yaz

        if (request.OrderItemsIds is null || request.OrderItemsIds.Count == 0)
            return BadRequest("No items provided.");

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized("Invalid or missing token.");

        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
        if (user is null)
            return Unauthorized("User not found or inactive.");

        Coupon? coupon = null;

        if (request.CouponCode is not null)
        {
            coupon = await context.Coupons
              .Where(c => c.Code == request.CouponCode && c.IsActive && !c.IsDeleted)
              .FirstOrDefaultAsync();


            if (coupon is null)
                return NotFound("The following coupon was not found.");

            if (coupon.ExpirationDate < DateTime.Now)
                return BadRequest("The coupon is expired.");

            if (coupon.UsageLimit <= coupon.UsedCount)
                return BadRequest("The usage limit is exceeded");

        }

        var books = await context.Books
            .Where(b => request.OrderItemsIds.Contains(b.Id) && !b.IsDeleted)
            .Select(b => new BookOrderDTO
            (
                b.Id,
                b.Name,
                decimal.Round(b.Price - (b.Price * b.Discount / 100), 2, MidpointRounding.ToPositiveInfinity)
            ))
            .ToArrayAsync();

        var totalPrice = books.Aggregate((decimal)0, (total, book) => total + book.Price);


        var newOrder = new Order
        {
            Id = Guid.CreateVersion7(),
            OrderItems = JsonSerializer.Serialize(books),
            TotalPrice = totalPrice,
            UserId = userId,

        };

        try
        {
            await context.Orders.AddAsync(newOrder);

            if (coupon is not null)
                coupon.UsedCount += 1;

            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(AddOrder), new { id = newOrder.Id }, new
            {
                newOrder.Id,
                newOrder.TotalPrice,
                newOrder.UserId,
                newOrder.OrderItems,
                newOrder.CreatedAt
            });
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

    [HttpGet, Authorize]
    public IActionResult GetMyOrders()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized("Invalid or missing token.");

        var orders = context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new
            {
                o.Id,
                Code = GenerateDisplayCode(o.Id),
                o.TotalPrice,
                o.CreatedAt
            });
        return Ok(orders);
    }


    //admin methodlari
    [HttpGet("all"), Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult GetAllOrders()
    {
        var orders = context.Orders
            .Include(o => o.User)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new
            {
                o.Id,
                Code = GenerateDisplayCode(o.Id),
                o.TotalPrice,
                o.CreatedAt,
                o.UserId,
                UserName = o.User.Name,
                UserEmail = o.User.Email
            });
            

        return Ok(orders);
    }

    [HttpGet("{id:guid}"), Authorize]
    public async Task<IActionResult> GetOrderDetail(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized("Invalid or missing token.");

        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order is null) 
            return NotFound("Order not found.");

        var role = User.FindFirstValue(ClaimTypes.Role);

        if (role != "Admin" && role != "SuperAdmin" && order.UserId != userId)
            return Forbid();

        var items = JsonSerializer.Deserialize<BookOrderDTO[]>(order.OrderItems) ?? [];

        var response = new
        {
            order.Id,
            Code = GenerateDisplayCode(order.Id),
            order.TotalPrice,
            order.CreatedAt,
            Items = items
        };

        return Ok(response);
    }

    [HttpDelete("{id:guid}"), Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> DeleteOrder(Guid id)
    {
        var order = await context.Orders.FirstOrDefaultAsync(o => o.Id == id);
        if (order is null)
            return NotFound("Order not found.");

        try
        {
            context.Orders.Remove(order);
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
