using Core.Entities;

namespace Entities.Concrete
{
    public class ProductImage : BaseEntity
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
    }
}
