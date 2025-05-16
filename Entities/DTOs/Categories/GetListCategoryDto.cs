using Core.Entities;

namespace Entities.DTOs.Categories
{
    public class GetListCategoryDto : IDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public ICollection<GetListCategoryDto>? ChildCategories { get; set; }
    }
}
