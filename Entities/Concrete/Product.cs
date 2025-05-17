using Core.Entities;
using Entities.Concrete.Shoppings;

namespace Entities.Concrete
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } // Ürün adı
        public string Description { get; set; } // Ürün açıklaması
        public decimal Price { get; set; } // Ürün fiyatı
        public int StockQuantity { get; set; } // Stok miktarı
        public int CategoryId { get; set; } // Ürünün bağlı olduğu kategori
        public int BrandId { get; set; } //Ürünün Bağlı Olduğu kategori

        // İlişkiler
        public Category Category { get; set; } // Ürünün kategorisi
        public Brand Brand { get; set; } // Ürünün Markası
        public List<BasketItem> BasketItems { get; set; } //Sepet
    }
}
