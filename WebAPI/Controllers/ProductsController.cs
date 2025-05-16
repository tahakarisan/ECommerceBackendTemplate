using Business.Abstract;
using Core.Utilities.Dynamic;
using Entities.Concrete;
using Entities.DTOs.Products;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        #region Queries
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Core.Utilities.Results.IDataResult<Product> result = await _productService.GetAsync(q => q.Id == id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync(int index, int size)
        {
            Core.Utilities.Results.IDataResult<Core.Utilities.Paging.IPaginate<Product>> products = await _productService.GetAllAsync(index: index, size: size);
            return products.Success ? Ok(products) : BadRequest(products);
        }
        [HttpGet("get-most-expensive-product")]
        public async Task<IActionResult> GetMostExpensiveProductAsync()
        {
            return Ok(await _productService.GetMostExpensiveProductAsync());
        }
        [HttpGet("get-product-details-dto")]
        public async Task<IActionResult> GetProductDetailDtoAsync(int index, int size)
        {
            Core.Utilities.Results.IDataResult<Core.Utilities.Paging.IPaginate<ProductDetailDto>> products = await _productService.GetProductDetailDtoAsync(index, size);
            return Ok(products);
        }
        [HttpGet("get-related-products-by-category-name")]
        public async Task<IActionResult> GetRelatedProductsByCategoryNameAsync(string categoryName, int index = 0, int size = 20)
        {
            Core.Utilities.Results.IDataResult<Core.Utilities.Paging.IPaginate<ProductDetailDto>> products = await _productService.GetRelatedProductsByCategoryNameAsync(categoryName, index: index, size: size);
            return Ok(products);
        }
        [HttpGet("get-related-products-by-product-id")]
        public async Task<IActionResult> GetRelatedProductsByProductIdAsync(int productId, int index = 0, int size = 20)
        {
            Core.Utilities.Results.IDataResult<Core.Utilities.Paging.IPaginate<ProductDetailDto>> products = await _productService.GetRelatedProductsByProductIdAsync(productId, index: index, size: size);
            return Ok(products);
        }
        [HttpGet("get-products-count-from-dal")]
        public async Task<IActionResult> GetProductsCountFromDalAsync()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            int result = await _productService.GetProductsCountFromDalAsync();
            stopwatch.Stop();
            return Ok($"Total Count :  {result}\nTotal Time : {stopwatch.ElapsedMilliseconds} ms");
        }
        [HttpGet("get-popular-products")]
        public async Task<IActionResult> GetPopularProductsAsync(int index = 0, int size = 20)
        {
            return Ok(await _productService.GetPopularProductsAsync(index, size));
        }
        [HttpGet("get-product-detail-by-id")]
        public async Task<IActionResult> GetProductDetailByIdAsync(int id)
        {
            return Ok(await _productService.GetProductDetailByIdAsync(id));
        }
        #endregion
        #region Commands
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddProductDto addProductDto)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            stopwatch.Start();
            Core.Utilities.Results.IResult result = await _productService.AddAsync(addProductDto);
            stopwatch.Stop();
            return result.Success ? Ok($"Total Count : {result}\nTotal Time : {stopwatch.ElapsedMilliseconds} ms") : BadRequest(result);
        }
        [HttpPost("add-range")]
        public async Task<IActionResult> AddRangeAsync(List<Product> products)
        {
            Core.Utilities.Results.IResult result = await _productService.AddRangeAsync(products);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(Product product)
        {
            Core.Utilities.Results.IResult result = await _productService.UpdateAsync(product);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(int productId)
        {
            Core.Utilities.Results.IResult result = await _productService.DeleteAsync(productId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("add-with-image")]
        public async Task<IActionResult> AddWithImageAsync([FromForm] AddProductWithImageDto addProductWithImageDto)
        {
            Core.Utilities.Results.IResult result = await _productService.AddWithImageAsync(addProductWithImageDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("get-product-details-paginate-dynamic")]
        public async Task<IActionResult> GetProductDetailsPaginateDynamicAsync(int index, int size, [FromBody] Dynamic dynamic)
        {
            Core.Utilities.Results.IDataResult<Core.Utilities.Paging.IPaginate<Product>> products = await _productService.GetListDynamicAsync(index, size, dynamic);
            return Ok(products);
        }
        #endregion
    }
}

