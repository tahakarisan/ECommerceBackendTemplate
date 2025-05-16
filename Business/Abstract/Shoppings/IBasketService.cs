using Core.Utilities.Paging;
using Core.Utilities.Results;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings;
using System.Linq.Expressions;

namespace Business.Abstract.Shoppings
{
    public interface IBasketService
    {
        Task<IResult> AddAsync(AddBasketDto addBasketDto);
        Task<IResult> UpdateAsync(Basket brand);
        Task<IResult> DeleteAsync(int id);
        Task<IDataResult<IPaginate<Basket>>> GetAllAsync(int index, int size);
        Task<IDataResult<Basket>> GetAsync(Expression<Func<Basket, bool>> filter);
    }
}
