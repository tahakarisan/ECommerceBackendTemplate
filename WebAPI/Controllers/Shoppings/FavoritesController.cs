using Business.Abstract.Shoppings;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings;
using Entities.DTOs.Shoppings.FavoriteModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Shoppings
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoritesController : Controller
    {
        private readonly IFavoriteService _favoriteService;
        public FavoritesController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        #region Queries
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync(int index, int size)
        {
            IDataResult<IPaginate<Favorite>> result = await _favoriteService.GetAllAsync(index, size);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            IDataResult<Favorite> result = await _favoriteService.GetAsync(q => q.Id == id);
            return result.Success ? Ok(result) : BadRequest();
        }

        #endregion

        #region Commands
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddFavoriteDto addFavoriteDto)
        {
            Core.Utilities.Results.IResult result = await _favoriteService.AddAsync(addFavoriteDto);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(Favorite favorite)
        {
            Core.Utilities.Results.IResult result = await _favoriteService.UpdateAsync(favorite);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Core.Utilities.Results.IResult result = await _favoriteService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest();
        }
        #endregion

    }
}
