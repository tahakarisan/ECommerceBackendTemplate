using Business.Abstract.Shoppings;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Shoppings;
using System.Linq.Expressions;

namespace Business.Concrete.Shoppings
{
    public class OrderManager : IOrderService
    {
        private IOrderDal _orderDal;
        public OrderManager(IOrderDal orderDal)
        {
            _orderDal = orderDal;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<Order>>> GetAllAsync(int index, int size)
        {
            IPaginate<Order>? result = await _orderDal.GetListAsync(index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<Order>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<Order>>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<Order>> GetAsync(Expression<Func<Order, bool>> filter)
        {
            Order? result = await _orderDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<Order>(result, Messages.Listed) : new ErrorDataResult<Order>(result, Messages.NotListed);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.IOrderService.GetAllAsync,
        Business.Abstract.IOrderService.GetAsync
        ")]
        public async Task<IResult> UpdateAsync(Order order)
        {
            bool isExists = await _orderDal.IsExistAsync(q => q.Id == order.Id);
            if (!isExists)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _orderDal.UpdateAsync(order);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IOrderService.GetAllAsync,
        Business.Abstract.IOrderService.GetAsync
        ")]
        public async Task<IResult> AddAsync(Order order)
        {
            int result = await _orderDal.AddAsync(order);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IOrderService.GetAllAsync,
        Business.Abstract.IOrderService.GetAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            Order deletedOrder = await _orderDal.GetAsync(q => q.Id == id);
            if (deletedOrder == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _orderDal.DeleteAsync(deletedOrder);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        #endregion
    }
}
