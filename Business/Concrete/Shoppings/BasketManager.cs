using AutoMapper;
using Business.Abstract.Shoppings;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Extensions;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Business.Concrete.Shoppings
{
    public class BasketManager : IBasketService
    {
        private IBasketDal _basketDal;
        private IMapper _mapper;
        public BasketManager(IBasketDal basketDal, IMapper mapper)
        {
            _basketDal = basketDal;
            _mapper = mapper;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<Basket>>> GetAllAsync(int index, int size)
        {
            Paginate<Basket> result = (Paginate<Basket>)await _basketDal.GetListAsync(
                index: index, size: size,
                include: i => i.Include(u => u.User).Include(b => b.Items).ThenInclude(p => p.Product)
                );

            Paginate<Basket> data = GeneralExtensions.ClearCircularReference<Paginate<Basket>>(result);

            return result != null ? new SuccessDataResult<IPaginate<Basket>>(data, Messages.Listed) : new ErrorDataResult<IPaginate<Basket>>(data, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<Basket>> GetAsync(Expression<Func<Basket, bool>> filter)
        {
            Basket? result = await _basketDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<Basket>(result, Messages.Listed) : new ErrorDataResult<Basket>(result, Messages.NotListed);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.IBasketService.GetAllAsync,
        Business.Abstract.IBasketService.GetAsync
        ")]
        public async Task<IResult> UpdateAsync(Basket basket)
        {
            bool isExists = await _basketDal.IsExistAsync(q => q.Id == basket.Id);
            if (!isExists)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _basketDal.UpdateAsync(basket);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IBasketService.GetAllAsync,
        Business.Abstract.IBasketService.GetAsync
        ")]
        public async Task<IResult> AddAsync(AddBasketDto addBasketDto)
        {
            Basket basket = _mapper.Map<Basket>(addBasketDto);
            int result = await _basketDal.AddAsync(basket);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IBasketService.GetAllAsync,
        Business.Abstract.IBasketService.GetAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            Basket deletedBasket = await _basketDal.GetAsync(q => q.Id == id);
            if (deletedBasket == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _basketDal.DeleteAsync(deletedBasket);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        #endregion
    }
}
