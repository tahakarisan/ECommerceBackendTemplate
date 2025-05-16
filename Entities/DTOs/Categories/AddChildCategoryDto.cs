namespace Entities.DTOs.Categories
{
    public class AddChildCategoryDto
    {
        public string Name { get; set; }
        public int ParentCategoryId { get; set; }
    }
}
