namespace Entities.DTOs.Categories.TrendyolDtos
{
    public class TrendyolCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } // Kategori adı
        public int? ParentId { get; set; } // Üst kategori (nullable)

        public ICollection<TrendyolCategoryDto> SubCategories { get; set; } // Alt kategoriler
    }
}
