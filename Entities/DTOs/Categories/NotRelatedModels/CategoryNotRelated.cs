namespace Entities.DTOs.Categories.NotRelatedModels
{
    public class CategoryNotRelated
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; } // Kategori adı
        public string Description { get; set; } // Kategori açıklaması (opsiyonel)
        public int? ParentCategoryId { get; set; } // Üst kategori (nullable)
    }
}
