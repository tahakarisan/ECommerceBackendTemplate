using Business.Abstract.Shoppings;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Shopping
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : ControllerBase
    {
        private readonly IBasketItemService _basketItemService;
        public BasketItemsController(IBasketItemService basketItemService)
        {
            _basketItemService = basketItemService;
        }
        #region Queries
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync(int index, int size)
        {
            Core.Utilities.Results.IDataResult<Core.Utilities.Paging.IPaginate<BasketItem>> result = await _basketItemService.GetAllAsync(index, size);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Core.Utilities.Results.IDataResult<BasketItem> result = await _basketItemService.GetAsync(q => q.Id == id);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpGet("get-basket-items-by-user-id")]
        public async Task<IActionResult> GetBasketItemsByIdUserId(int userId)
        {
            Core.Utilities.Results.IDataResult<List<BasketItem>> result = await _basketItemService.GetBasketItemsByIdUserIdAsync(userId);
            return result.Success ? Ok(result) : BadRequest();
        }
        #endregion
        #region Commands
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddBasketItemDto basketItemDto)
        {
            Core.Utilities.Results.IResult result = await _basketItemService.AddAsync(basketItemDto);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(BasketItem basketItem)
        {
            Core.Utilities.Results.IResult result = await _basketItemService.UpdateAsync(basketItem);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Core.Utilities.Results.IResult result = await _basketItemService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest();
        }
        #endregion
    }
}
