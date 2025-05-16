namespace Entities.DTOs.Categories
{
    public class GetChildCategoriesDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GetChildCategoriesDto> ChildCategories { get; set; }
    }
}
