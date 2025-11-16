using Project.Api.Application.DTOs;

namespace Project.Api.Domain.Entities
{
    public class Order : AuditableEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; }

        public string OrderItems { get; set; }  //List<orderitemdto>

        public decimal TotalPrice { get; set; }
    }
}
