using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.Api.Application.DTOs;
using Project.Api.Application.Services.Abstractions;
using Project.Api.Domain.Entities;
using Project.Api.Persistence.Contexts;
using Project.Api.Persistence.Repositories.Books;
using Project.Api.Persistence.Repositories.Coupons;
using Project.Api.Persistence.Repositories.Orders;
using Project.Api.Persistence.Repositories.Users;
using System.Security.Claims;
using System.Text.Json;

namespace Project.Api.Application.Services.Implementations;

public sealed class OrderService(IOrderRepository orderRepository,
                        ICouponRepository couponRepository,
                        IBookRepository bookRepository,
                        IUserRepository userRepository,
                        IHttpContextAccessor accessor) : IOrderService
{
    private readonly ClaimsPrincipal User = accessor.HttpContext?.User ?? throw new InvalidDataException("No user found.");

    private static string GenerateDisplayCode(Guid id) => $"ORDER-{id.ToString("N")[..8].ToUpper()}";


    public async Task<AddOrderResponse> AddOrderAsync(AddOrderRequest request)
    {
        //OrderItemDTO

        // check ele, ele bir user var mi
        // tokenle olsun
        // try catch, databazaya yazilsin
        //2 alqoritma yaz

        if (request.OrderItems is null || request.OrderItems.Count == 0)
            throw new NullReferenceException("No items provided.");

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new InvalidDataException("Invalid or missing token.");

        var user = await userRepository.FindAsync(userId);
        if (user is null && user!.IsDeleted)
            throw new InvalidDataException("User not found or inactive.");

        Coupon? coupon = null;

        if (request.CouponCode is not null)
        {
            coupon = await couponRepository
              .GetWhereAll(c => c.Code == request.CouponCode && c.IsActive && !c.IsDeleted)
              .FirstOrDefaultAsync();


            if (coupon is null)
                throw new NullReferenceException("The following coupon was not found.");

            if (coupon.ExpirationDate < DateTime.Now)
                throw new InvalidOperationException("The coupon is expired.");

            if (coupon.UsageLimit <= coupon.UsedCount)
                throw new InvalidOperationException("The usage limit is exceeded");

        }

        var booksTable = bookRepository
            .GetWhereAll(b => request.OrderItems.Select(oi => oi.Id)
            .Contains(b.Id) && !b.IsDeleted);


        var books = await booksTable
            .Select(b => new BookOrderDTO
            (
                b.Id,
                b.Name,
                decimal.Round(b.Price - (b.Price * b.Discount / 100), 2, MidpointRounding.ToPositiveInfinity),
                b.Stock
            ))
            .ToArrayAsync();

        decimal totalPrice = 0;

        foreach (var book in books)
        {
            foreach (var req in request.OrderItems)
            {
                if (req.Id == book.Id)
                {
                    if (req.Quantity > book.Stock)
                    {
                        throw new InvalidOperationException("The desired quantity of this product exceeds our current stock.");
                    }

                    totalPrice = decimal.Round(totalPrice + book.Price * req.Quantity * (coupon is not null ? 1 - ((decimal)coupon.DiscountPercentage / 100) : 1), 2, MidpointRounding.ToPositiveInfinity);

                }
            }
        }

        var newOrderId = Guid.CreateVersion7();

        var newOrder = new Order
        {
            Id = newOrderId,
            OrderItems = JsonSerializer.Serialize(request.OrderItems),
            DisplayCode = GenerateDisplayCode(newOrderId),
            CouponCode = request.CouponCode,
            TotalPrice = totalPrice,
            UserId = userId,
        };


        await using var transaction = await orderRepository.BeginTransactionAsync();

        try
        {
            await orderRepository.AddAsync(newOrder);

            var booksToUpdate = await booksTable.ToListAsync();

            foreach (var book in booksTable)
            {
                foreach (var req in request.OrderItems)
                {
                    if (req.Id == book.Id)
                    {
                        book.Stock -= req.Quantity;

                    }
                }
            }


            if (coupon is not null)
                coupon.UsedCount += 1;

            await orderRepository.SaveChangesAsync();
            await transaction.CommitAsync();

        }
        catch (DbUpdateException)
        {
            await transaction.RollbackAsync();
            throw;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }

        return new AddOrderResponse(newOrder.Id,
            newOrder.TotalPrice,
            newOrder.UserId,
            JsonSerializer.Deserialize<List<OrderItem>>(newOrder.OrderItems)!,         //to fix later, can throw an exception
            GenerateDisplayCode(newOrder.Id),
            newOrder.CouponCode,
            newOrder.CreatedAt);
    }

    public IEnumerable<AllOrdersDBModel> GetAllOrders()
    {
        return orderRepository
             .GetOrderWithUser()
             .Select(o => new AllOrdersDBModel
             (
                 o.Id,
                 JsonSerializer.Deserialize<List<OrderItem>>(o.OrderItems)!,
                o.CouponCode,
                o.TotalPrice,
                o.DisplayCode,
                o.CreatedAt,
                o.UserId,
                o.User.Name,
                o.User.Email
             ));
    }


    public IEnumerable<MyOrdersResponse> GetMyOrders()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new InvalidDataException("Invalid or missing token.");

        var userExists = userRepository.GetWhereAll(u => u.Id == userId && !u.IsDeleted)
                                        .Any();
        if (!userExists)
            throw new InvalidDataException("User not found or inactive.");

        return orderRepository
            .GetWhereAll(o => o.UserId == userId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new MyOrdersResponse
            (
                o.Id,
                o.DisplayCode,
                o.CouponCode,
                o.TotalPrice,
                o.CreatedAt
            ));
    }

    public async Task<OrderDetailResponse> GetOrderDetailAsync(Guid id)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            throw new InvalidDataException("Invalid or missing token.");


        var order = await orderRepository
            .GetWhereAll(o => o.Id == id)
            .FirstOrDefaultAsync();

        if (order is null)
            throw new NullReferenceException("Order not found.");

        var items = JsonSerializer.Deserialize<List<OrderItem>>(order.OrderItems) ?? [];

        return new OrderDetailResponse(
        order.Id,
        order.DisplayCode,
        order.TotalPrice,
        items,
        order.CouponCode,
        order.CreatedAt
    );

    }
}
