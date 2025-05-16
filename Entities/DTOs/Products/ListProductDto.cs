using Core.Entities;
using Entities.Concrete;

namespace Entities.DTOs.Products
{
    public class ListProductDto : IDto
    {
        public Product Product { get; set; }
        public List<ProductImage> ProductImages { get; set; }

    }
}
