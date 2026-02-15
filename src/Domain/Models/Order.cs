namespace Domain.Models;

public class Order : AuditableEntity<Guid>, ISoftDelete
{
    public Guid UserId { get; set; }
    public User User { get; set; }
    public string DisplayCode { get; set; }

    public string? CouponCode { get; set; }

    public string OrderItems { get; set; }  //List<orderitemdto>

    public decimal TotalPrice { get; set; }
    public bool IsDeleted { get; set; }
}
