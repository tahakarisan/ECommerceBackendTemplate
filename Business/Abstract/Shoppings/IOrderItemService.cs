using Core.Utilities.Paging;
using Core.Utilities.Results;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings;
using System.Linq.Expressions;

namespace Business.Abstract.Shoppings
{
    public interface IOrderItemService
    {
        Task<IResult> AddAsync(AddOrderItemDto addOrderItemDto);
        Task<IResult> UpdateAsync(OrderItem orderItem);
        Task<IResult> DeleteAsync(int id);
        Task<IDataResult<IPaginate<OrderItem>>> GetAllAsync(int index, int size);
        Task<IDataResult<OrderItem>> GetAsync(Expression<Func<OrderItem, bool>> filter);
    }
}
