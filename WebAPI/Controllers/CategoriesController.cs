using Business.Abstract;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Categories;
using Entities.DTOs.Categories.TrendyolDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text.Json;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        #region Queries
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync(int index, int size)
        {
            Core.Utilities.Results.IDataResult<Core.Utilities.Paging.Paginate<CategoryDto>> result = await _categoryService.GetAllAsync(index: index, size: size);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpGet("get-child-categories-by-category-id")]
        public async Task<IActionResult> GetChildCategoriesByCategoryIdAsync(int categoryId)
        {
            Core.Utilities.Results.IDataResult<List<CategoryDto>> result = await _categoryService.GetChildCategoriesByCategoryIdAsync(categoryId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetByIdAsync(int categoryId)
        {
            Core.Utilities.Results.IDataResult<CategoryDto> result = await _categoryService.GetAsync(q => q.Id == categoryId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        #endregion
        #region Commands
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddCategoryDto addCategoryDto)
        {
            Core.Utilities.Results.IDataResult<CategoryDto> result = await _categoryService.AddWithDtoAsync(addCategoryDto);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(Category category)
        {
            Core.Utilities.Results.IResult result = await _categoryService.UpdateAsync(category);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(int categoryId)
        {
            Core.Utilities.Results.IResult result = await _categoryService.DeleteAsync(categoryId);
            return result.Success ? Ok(result) : BadRequest(result);
        }
        [HttpPost("add-categories-api")]
        public async Task<IActionResult> AddCategoriesApiAsync()
        {
            //sadece ihtiyac duyulduğunda çalıştırılması gerek
            //return null;

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("https://api.trendyol.com/sapigw/product-categories");

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    string responseContent = await response.Content.ReadAsStringAsync();

                    TrendyolCategoryResponseDto apiRoot = JsonConvert.DeserializeObject<TrendyolCategoryResponseDto>(responseContent);
                    IEnumerable<TrendyolCategoryDto> apiCategories = apiRoot.Categories;
                    foreach (TrendyolCategoryDto category in apiCategories)
                    {
                        await _categoryService.AddCategoryAsync(category);
                    }
                }
                catch (Exception)
                {
                }
            }
            else { }
            return null;

        }
        #endregion
    }
}
