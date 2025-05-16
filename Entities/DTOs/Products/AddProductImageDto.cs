using Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Entities.DTOs.Products
{
    public class AddProductImageDto : IDto
    {
        public int ProductId { get; set; }
        public IFormFileCollection IFormFiles { get; set; }
    }
}
