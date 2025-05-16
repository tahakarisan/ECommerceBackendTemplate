using AutoMapper;
using Business.Abstract.Shoppings;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings;
using System.Linq.Expressions;

namespace Business.Concrete.Shoppings
{
    public class OrderItemManager : IOrderItemService
    {
        private IOrderItemDal _orderItemDal;
        private IMapper _mapper;
        public OrderItemManager(IOrderItemDal orderItemDal, IMapper mapper)
        {
            _orderItemDal = orderItemDal;
            _mapper = mapper;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<OrderItem>>> GetAllAsync(int index, int size)
        {
            IPaginate<OrderItem>? result = await _orderItemDal.GetListAsync(index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<OrderItem>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<OrderItem>>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<OrderItem>> GetAsync(Expression<Func<OrderItem, bool>> filter)
        {
            OrderItem? result = await _orderItemDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<OrderItem>(result, Messages.Listed) : new ErrorDataResult<OrderItem>(result, Messages.NotListed);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.IOrderItemService.GetAllAsync,
        Business.Abstract.IOrderItemService.GetAsync
        ")]
        public async Task<IResult> UpdateAsync(OrderItem orderItem)
        {
            bool isExists = await _orderItemDal.IsExistAsync(q => q.Id == orderItem.Id);
            if (!isExists)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _orderItemDal.UpdateAsync(orderItem);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IOrderItemService.GetAllAsync,
        Business.Abstract.IOrderItemService.GetAsync
        ")]
        public async Task<IResult> AddAsync(AddOrderItemDto addOrderItemDto)
        {
            OrderItem orderItem = _mapper.Map<OrderItem>(addOrderItemDto);
            int result = await _orderItemDal.AddAsync(orderItem);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IOrderItemService.GetAllAsync,
        Business.Abstract.IOrderItemService.GetAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            OrderItem deletedOrderItem = await _orderItemDal.GetAsync(q => q.Id == id);
            if (deletedOrderItem == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _orderItemDal.DeleteAsync(deletedOrderItem);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        #endregion
    }
}
