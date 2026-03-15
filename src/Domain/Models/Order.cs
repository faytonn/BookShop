namespace Domain.Models;

public class Order : AuditableEntity<Guid>, ISoftDelete
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string DisplayCode { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string? CouponCode { get; set; }
    public ShippingAddress ShippingAddress { get; set; } = null!;

    public string OrderItems { get; set; }  //List<orderitemdto>

    public decimal TotalPrice { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; } = true;

    public List<OrderHistory> OrderHistories { get; set; } = [];
}


public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    OutForDelivery,
    Delivered,
    Canceled
}