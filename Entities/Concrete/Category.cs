using Core.Entities;

namespace Entities.Concrete
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } // Kategori adı
        public string Description { get; set; } // Kategori açıklaması (opsiyonel)
        public int? ParentCategoryId { get; set; } // Üst kategori (nullable)

        // İlişkiler
        public Category ParentCategory { get; set; } // Üst kategori
        public ICollection<Category> SubCategories { get; set; } // Alt kategoriler
        public ICollection<Product> Products { get; set; } // Bu kategorideki ürünler
    }
}
