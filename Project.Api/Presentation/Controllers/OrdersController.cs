using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;
using Project.Api.Domain.Entities;
using Project.Api.Persistence.Contexts;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace Project.Api.Presentation.Controllers;

[Route("api/v1/orders"), ApiController]

public sealed class OrdersController(AppDbContext context, IOrderService orderService) : ControllerBase
{
    private static string GenerateDisplayCode(Guid id) => $"ORDER{id.ToString("N")[..8].ToUpper()}";

    // ORDER6754GBGSF-5656-34343

    [HttpPost, Authorize]
    public async Task<IActionResult> AddOrder(AddOrderRequest request)
    {
        try
        {
            var newOrder = await orderService.AddOrderAsync(request);

            return CreatedAtAction(nameof(AddOrder), new { id = newOrder.Id }, newOrder);
        }
        catch (NullReferenceException ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.NotFound);
        }
        catch (InvalidDataException ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.Unauthorized);
        }
        catch (Exception ex) when (ex is InvalidOperationException || ex is DbUpdateException)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.BadRequest);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }

    [HttpGet("my"), Authorize]
    public async Task<IActionResult> GetMyOrders()
    {
        try
        {
            var result = orderService.GetMyOrders(); 
            return Ok(result);
        }
        catch (InvalidDataException ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.Unauthorized);
        }
        catch (Exception ex)
        {
            return Problem(detail: ex.Message, statusCode: (int)HttpStatusCode.InternalServerError);
        }
    }



    //admin methodlari
    [HttpGet("all"), Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult GetAllOrders()
    {
        var allOrders = orderService.GetAllOrders();

        return Ok(allOrders);
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
