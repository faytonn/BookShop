using Project.Api.Application.DTOs;
using Project.Api.Domain.Entities;

namespace Project.Api.Application.Services.Abstractions;

public interface IOrderService
{
    Task<AddOrderResponse> AddOrderAsync(AddOrderRequest request);
    IEnumerable<MyOrdersResponse> GetMyOrders();
    IEnumerable<AllOrdersDBModel> GetAllOrders();
    Task<OrderDetailResponse> GetOrderDetailAsync(Guid id);

    //Task<(Order order, BookOrderDTO[] items)?> GetOrderWithItemsAsync(Guid id);
    //bool DeleteOrder(Guid id);
}
