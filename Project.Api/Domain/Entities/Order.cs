using Project.Api.Application.DTOs;
using Project.Api.Domain.Entities.Commons;

namespace Project.Api.Domain.Entities
{
    public class Order : AuditableEntity, ISoftDelete
    {
        public Guid UserId { get; set; }
        public User User { get; set; }
        public string DisplayCode { get; set; }

        public string? CouponCode { get; set; }

        public string OrderItems { get; set; }  //List<orderitemdto>

        public decimal TotalPrice { get; set; }
        public bool IsDeleted { get; set; }
    }
}
