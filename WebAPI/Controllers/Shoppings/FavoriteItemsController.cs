using Business.Abstract.Shoppings;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings;
using Entities.DTOs.Shoppings.FavoriteModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Shoppings
{
    [Route("api/[Controller]")]
    [ApiController]
    public class FavoriteItemsController : Controller
    {
        private readonly IFavoriteItemService _favoriteItemService;
        public FavoriteItemsController(IFavoriteItemService favoriteItemService)
        {
               _favoriteItemService = favoriteItemService;
        }

        #region Queries
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync(int index, int size)
        {
            Core.Utilities.Results.IDataResult<Core.Utilities.Paging.IPaginate<FavoriteItem>> result = await _favoriteItemService.GetAllAsync(index, size);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Core.Utilities.Results.IDataResult<FavoriteItem> result = await _favoriteItemService.GetAsync(q => q.Id == id);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpGet("get-favorite-items-by-user-id")]
        public async Task<IActionResult> GetBasketItemsByIdUserId(int userId)
        {
            Core.Utilities.Results.IDataResult<List<FavoriteItem>> result = await _favoriteItemService.GetFavoriteItemsByIdUserIdAsync(userId);
            return result.Success ? Ok(result) : BadRequest();
        }
        #endregion

        #region Commands

        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddFavoriteItemDto addFavoriteItemDto)
        {
            Core.Utilities.Results.IResult result = await _favoriteItemService.AddAsync(addFavoriteItemDto);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(FavoriteItem favoriteItem)
        {
            Core.Utilities.Results.IResult result = await _favoriteItemService.UpdateAsync(favoriteItem);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Core.Utilities.Results.IResult result = await _favoriteItemService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest();
        }
        #endregion
    }
}
