using Project.Api.Domain.Entities.Commons;

namespace Project.Api.Domain.Entities;

public class Coupon : AuditableEntity, ISoftDelete
{
    public string Code { get; set; } = null!;
    public byte DiscountPercentage { get; set; }
    public DateTime ExpirationDate { get; set; }
    public int UsageLimit { get; set; }
    public int UsedCount { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsDeleted { get; set; }
}
