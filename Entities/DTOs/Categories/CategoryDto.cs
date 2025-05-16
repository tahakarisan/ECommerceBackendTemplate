using Entities.DTOs.Categories.NotRelatedModels;
using Entities.DTOs.Products;

namespace Entities.DTOs.Categories
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; } // Kategori adı
        public string Description { get; set; } // Kategori açıklaması (opsiyonel)
        public int? ParentCategoryId { get; set; } // Üst kategori (nullable)

        // İlişkiler
        public CategoryNotRelated ParentCategory { get; set; } // Üst kategori
        public ICollection<CategoryNotRelated> SubCategories { get; set; } // Alt kategoriler
        public ICollection<ProductDto> Products { get; set; } // Bu kategorideki ürünler
    }
}
