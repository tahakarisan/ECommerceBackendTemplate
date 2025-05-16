namespace Entities.DTOs.Products
{
    public class BrandDto
    {
        public int Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Name { get; set; }
        public ICollection<ProductDto> Products { get; set; } // Bu kategorideki ürünler
    }
}
