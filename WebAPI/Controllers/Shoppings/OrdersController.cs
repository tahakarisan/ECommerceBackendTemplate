using Business.Abstract.Shoppings;
using Entities.Concrete.Shoppings;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers.Shopping
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        #region Queries
        [HttpGet("getall")]
        public async Task<IActionResult> GetAllAsync(int index, int size)
        {
            Core.Utilities.Results.IDataResult<Core.Utilities.Paging.IPaginate<Order>> result = await _orderService.GetAllAsync(index, size);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpGet("get-by-id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            Core.Utilities.Results.IDataResult<Order> result = await _orderService.GetAsync(q => q.Id == id);
            return result.Success ? Ok(result) : BadRequest();
        }
        #endregion
        #region Commands
        [HttpPost("add")]
        public async Task<IActionResult> AddAsync(Order order)
        {
            Core.Utilities.Results.IResult result = await _orderService.AddAsync(order);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpPost("update")]
        public async Task<IActionResult> UpdateAsync(Order order)
        {
            Core.Utilities.Results.IResult result = await _orderService.UpdateAsync(order);
            return result.Success ? Ok(result) : BadRequest();
        }
        [HttpPost("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Core.Utilities.Results.IResult result = await _orderService.DeleteAsync(id);
            return result.Success ? Ok(result) : BadRequest();
        }
        #endregion
    }
}
