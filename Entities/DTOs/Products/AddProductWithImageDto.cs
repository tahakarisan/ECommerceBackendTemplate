using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs.Products
{
    public class AddProductWithImageDto : IDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int StockQuantity { get; set; }
        public int CategoryId { get; set; }
        public IFormFileCollection IFormFiles { get; set; }
    }
}
