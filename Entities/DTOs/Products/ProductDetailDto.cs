using Core.Entities;

namespace Entities.DTOs.Products
{
    public class ProductDetailDto : BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<string> ProductImages { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
    }
}
