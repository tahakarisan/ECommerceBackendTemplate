using AutoMapper;
using Business.Abstract.Shoppings;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Shoppings;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Shoppings.FavoriteModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete.Shoppings
{
    public class FavoriteItemManager : IFavoriteItemService
    {
        private readonly IMapper _mapper;
        private readonly IFavoriteDal _favoriteDal;
        private readonly IFavoriteItemDal _favoriteItemDal;

        public FavoriteItemManager(IMapper mapper, IFavoriteItemDal favoriteItemDal, IFavoriteDal favoriteDal)
        {
            _mapper = mapper;
            _favoriteItemDal = favoriteItemDal;
            _favoriteDal = favoriteDal;
        }

        #region Queries

        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<FavoriteItem>>> GetAllAsync(int index, int size)
        {
            IPaginate<FavoriteItem> result = await _favoriteItemDal.GetListAsync(index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<FavoriteItem>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<FavoriteItem>>(Messages.NotListed);
        }

        [CacheAspect(60)]
        public async Task<IDataResult<FavoriteItem>> GetAsync(Expression<Func<FavoriteItem, bool>> filter)
        {
            FavoriteItem? result = await _favoriteItemDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<FavoriteItem>(result, Messages.Listed) : new ErrorDataResult<FavoriteItem>(Messages.NotListed);
        }

        [CacheAspect(60)]
        public async Task<IDataResult<List<FavoriteItem>>> GetFavoriteItemsByIdUserIdAsync(int userId)
        {
            List<FavoriteItem> result = await _favoriteItemDal.GetAllWithIncludeAsync(
               include:
                   i => i.Include(b => b.Favorite),
               filter: p => p.UserId == userId);
            return result != null ? new SuccessDataResult<List<FavoriteItem>>(result, Messages.Listed) : new ErrorDataResult<List<FavoriteItem>>(Messages.NotListed);
        }
        #endregion


        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.IFavoriteItemService.GetAllAsync,
        Business.Abstract.IFavoriteItemService.GetAsync,
        Business.Abstract.IFavoriteItemService.GetFavoriteItemsByIdUserIdAsync,
        ")]
        public async Task<IResult> AddAsync(AddFavoriteItemDto favoriteItemDto)
        {
            FavoriteItem? doesUserHaveBasket = await _favoriteItemDal.GetAsync(p => p.UserId == favoriteItemDto.UserId);
            if (doesUserHaveBasket == null)
            {
                favoriteItemDto.FavoriteId = await _favoriteDal.AddAsync(new Favorite { UserId = favoriteItemDto.UserId });
            }
            else
            {
                favoriteItemDto.FavoriteId = doesUserHaveBasket.Id;
            }
            FavoriteItem favoriteItem = _mapper.Map<FavoriteItem>(favoriteItemDto);
            int result = await _favoriteItemDal.AddAsync(favoriteItem);
            return result > 0 ? new SuccessResult(Messages.AddToBasket) : new ErrorResult(Messages.NotAdded);
        }


        [CacheRemoveAspect(@"
        Business.Abstract.IFavoriteItemService.GetAllAsync,
        Business.Abstract.IFavoriteItemService.GetAsync,
        Business.Abstract.IFavoriteItemService.GetFavoriteItemsByIdUserIdAsync,
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            IDataResult<FavoriteItem> deletedFavoriteItem = await GetAsync(q => q.Id == id);
            if (deletedFavoriteItem == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _favoriteItemDal.DeleteAsync(deletedFavoriteItem.Data);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }

        [CacheRemoveAspect(@"
        Business.Abstract.IFavoriteItemService.GetAllAsync,
        Business.Abstract.IFavoriteItemService.GetAsync,
        Business.Abstract.IFavoriteItemService.GetFavoriteItemsByIdUserIdAsync,
        ")]
        public async Task<IResult> UpdateAsync(FavoriteItem favoriteItem)
        {
            bool isExists = await _favoriteItemDal.IsExistAsync(q => q.Id == favoriteItem.Id);
            if (!isExists)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _favoriteItemDal.UpdateAsync(favoriteItem);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        #endregion

    }
}
