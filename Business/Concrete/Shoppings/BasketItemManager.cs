using AutoMapper;
using Business.Abstract.Shoppings;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Business.Concrete.Shoppings
{
    public class BasketItemManager : IBasketItemService
    {
        private IBasketItemDal _basketItemDal;
        private IBasketDal _basketDal;
        private IMapper _mapper;
        public BasketItemManager(IBasketItemDal basketItemDal, IMapper mapper, IBasketDal basketDal)
        {
            _basketItemDal = basketItemDal;
            _mapper = mapper;
            _basketDal = basketDal;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<BasketItem>>> GetAllAsync(int index, int size)
        {
            IPaginate<BasketItem> result = await _basketItemDal.GetListAsync(index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<BasketItem>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<BasketItem>>(Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<BasketItem>> GetAsync(Expression<Func<BasketItem, bool>> filter)
        {
            BasketItem? result = await _basketItemDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<BasketItem>(result, Messages.Listed) : new ErrorDataResult<BasketItem>(Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<List<BasketItem>>> GetBasketItemsByIdUserIdAsync(int userId)
        {
            List<BasketItem> result = await _basketItemDal.GetAllWithIncludeAsync(
                include:
                    i => i.Include(b => b.Basket),
                filter: p => p.UserId == userId);
            return result != null ? new SuccessDataResult<List<BasketItem>>(result, Messages.Listed) : new ErrorDataResult<List<BasketItem>>(Messages.NotListed);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.IBasketItemService.GetAllAsync,
        Business.Abstract.IBasketItemService.GetAsync,
        Business.Abstract.IBasketItemService.GetBasketItemsByIdUserIdAsync
        ")]
        public async Task<IResult> UpdateAsync(BasketItem basketItem)
        {
            bool isExists = await _basketItemDal.IsExistAsync(q => q.Id == basketItem.Id);
            if (!isExists)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _basketItemDal.UpdateAsync(basketItem);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IBasketItemService.GetAllAsync,
        Business.Abstract.IBasketItemService.GetAsync,
        Business.Abstract.IBasketItemService.GetBasketItemsByIdUserIdAsync
        ")]
        public async Task<IResult> AddAsync(AddBasketItemDto basketItemDto)
        {
            Basket? doesUserHaveBasket = await _basketDal.GetAsync(p => p.UserId == basketItemDto.UserId);
            if (doesUserHaveBasket == null)
            {
                basketItemDto.BasketId = await _basketDal.AddAsync(new Basket { UserId = basketItemDto.UserId });
            }
            else
            {
                basketItemDto.BasketId = doesUserHaveBasket.Id;
            }
            BasketItem basketItem = _mapper.Map<BasketItem>(basketItemDto);
            int result = await _basketItemDal.AddAsync(basketItem);
            return result > 0 ? new SuccessResult(Messages.AddToBasket) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IBasketItemService.GetAllAsync,
        Business.Abstract.IBasketItemService.GetAsync,
        Business.Abstract.IBasketItemService.GetBasketItemsByIdUserIdAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            IDataResult<BasketItem> deletedBasketItem = await GetAsync(q => q.Id == id);
            if (deletedBasketItem == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _basketItemDal.DeleteAsync(deletedBasketItem.Data);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        #endregion
    }
}
