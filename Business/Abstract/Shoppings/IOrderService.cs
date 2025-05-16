using Core.Utilities.Paging;
using Core.Utilities.Results;
using Entities.Concrete.Shoppings;
using System.Linq.Expressions;

namespace Business.Abstract.Shoppings
{
    public interface IOrderService
    {
        Task<IResult> AddAsync(Order order);
        Task<IResult> UpdateAsync(Order order);
        Task<IResult> DeleteAsync(int id);
        Task<IDataResult<IPaginate<Order>>> GetAllAsync(int index, int size);
        Task<IDataResult<Order>> GetAsync(Expression<Func<Order, bool>> filter);
    }
}
