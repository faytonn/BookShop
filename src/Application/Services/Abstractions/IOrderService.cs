namespace Application.Services.Abstractions;

public interface IOrderService
{
    Task<AddOrderResponse> AddOrderAsync(AddOrderRequest request);
    Task<CreateCheckoutSessionResponse> CreateCheckoutSessionAsync(Guid orderId);
    Task<UpdateOrderStatusResponse> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusRequest request);
    IEnumerable<MyOrdersResponse> GetMyOrders();
    IEnumerable<AllOrdersDBModel> GetAllOrders();
    Task<OrderDetailResponse> GetOrderDetailAsync(Guid id);
    //Task<(Order order, BookOrderDTO[] items)?> GetOrderWithItemsAsync(Guid id);
    //bool DeleteOrder(Guid id);
    Task CancelOrderAsync(Guid orderId);
    Task DeleteOrderAsync(Guid orderId);

}
