﻿namespace Entities.DTOs.Categories
{
    public class AddCategoryDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
