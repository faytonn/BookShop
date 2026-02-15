namespace Domain.Models;


public class Coupon : AuditableEntity<Guid>, ISoftDelete
{
    public string Code { get; set; } = null!;
    public byte DiscountPercentage { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int UsageLimit { get; set; }
    public int UsedCount { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
}
