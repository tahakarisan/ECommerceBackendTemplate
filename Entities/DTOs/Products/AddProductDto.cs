using Core.Entities;

namespace Entities.DTOs.Products
{
    public class AddProductDto : IDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
    }
}
