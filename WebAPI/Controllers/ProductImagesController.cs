using Business.Abstract;
using Entities.DTOs.Products;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductImagesController : ControllerBase
    {
        private IProductImageService _productImageService;

        public ProductImagesController(IProductImageService productImageService)
        {
            _productImageService = productImageService;
        }

        [HttpPost("add-product-image-by-product-id")]
        public async Task<IActionResult> AddProductImageByProductIdAsync([FromForm] AddProductImageDto addProductImageDto)
        {
            Core.Utilities.Results.IResult result = await _productImageService.AddProductImageByProductIdAsync(addProductImageDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
