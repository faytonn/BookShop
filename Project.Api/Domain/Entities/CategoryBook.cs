using Project.Api.Domain.Entities.Commons;

namespace Project.Api.Domain.Entities
{
    public class CategoryBook : Entity
    {
        public Guid BookId { get; set; }
        public Book Book { get; set; } 

        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
