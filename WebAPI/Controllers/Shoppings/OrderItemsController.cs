using Business.Abstract.Shoppings;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Shopping
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {

        private readonly IOrderItemService _orderItemService;
        public OrderItemsController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }
        #region Queries
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync(int index, int size)
        {
            Core.Utilities.Results.IDataResult<Core.Utilities.Paging.IPaginate<OrderItem>> result = await _orderItemService.GetAllAsync(index, size);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Core.Utilities.Results.IDataResult<OrderItem> result = await _orderItemService.GetAsync(q => q.Id == id);
            return result.Success ? Ok(result) : BadRequest();
        }
        #endregion
        #region Commands
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(AddOrderItemDto addOrderItemDto)
        {
            Core.Utilities.Results.IResult result = await _orderItemService.AddAsync(addOrderItemDto);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(OrderItem orderItem)
        {
            Core.Utilities.Results.IResult result = await _orderItemService.UpdateAsync(orderItem);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Core.Utilities.Results.IResult result = await _orderItemService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest();
        }
        #endregion
    }
}
