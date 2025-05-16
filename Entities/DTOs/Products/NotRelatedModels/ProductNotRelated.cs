namespace Entities.DTOs.Products
{
    public class ProductNotRelated
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; } // Ürün adı
        public string Description { get; set; } // Ürün açıklaması
        public decimal Price { get; set; } // Ürün fiyatı
        public int StockQuantity { get; set; } // Stok miktarı
        public int CategoryId { get; set; } // Ürünün bağlı olduğu kategori
        public int BrandId { get; set; }
    }
}
