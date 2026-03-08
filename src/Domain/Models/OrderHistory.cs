namespace Domain.Models;

public class OrderHistory : AuditableEntity<Guid>
{
    public Guid OrderId { get; set; }
    public Order Order { get; set; }

    public OrderStatus? FromStatus { get; set; }
    public OrderStatus ToStatus {  get; set; }

    public User? ChangedBy { get; set; }
    public Guid? ChangedByUserId { get; set; }


    public string? Description { get; set; }
}
