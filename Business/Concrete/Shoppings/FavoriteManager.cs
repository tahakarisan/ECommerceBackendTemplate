using AutoMapper;
using Business.Abstract.Shoppings;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Extensions;
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
    public class FavoriteManager : IFavoriteService
    {
        IFavoriteDal _favoriteDal;
        IMapper _mapper;
        public FavoriteManager(IFavoriteDal favoriteDal,IMapper mapper)
        {
            _favoriteDal=favoriteDal;
            _mapper=mapper;
            
        }

        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<Favorite>>> GetAllAsync(int index, int size)
        {
            Paginate<Favorite> result = (Paginate<Favorite>) await _favoriteDal.GetListAsync(
                    index: index, size: size,
                    include: i => i.Include(u => u.User).Include(b => b.FavoriteItems).ThenInclude(p => p.Product)
                    );
            Paginate<Favorite> data = GeneralExtensions.ClearCircularReference<Paginate<Favorite>>(result);

            return result != null ? new SuccessDataResult<IPaginate<Favorite>>(data, Messages.Listed) : new ErrorDataResult<IPaginate<Favorite>>(data, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<Favorite>> GetAsync(Expression<Func<Favorite, bool>> filter)
        {
            Favorite? result = await _favoriteDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<Favorite>(result, Messages.Listed) : new ErrorDataResult<Favorite>(result, Messages.NotListed);
        }

        #endregion

        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.IFavoriteService.GetAllAsync,
        Business.Abstract.IFavoriteService.GetAsync
        ")]
        public async Task<IResult> AddAsync(AddFavoriteDto addFavoriteDto)
        {
            Favorite favorite = _mapper.Map<Favorite>(addFavoriteDto);
            int result = await _favoriteDal.AddAsync(favorite);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IFavoriteService.GetAllAsync,
        Business.Abstract.IFavoriteService.GetAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            Favorite deletedFavorite = await _favoriteDal.GetAsync(q=>q.Id== id);
            if (deletedFavorite == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _favoriteDal.DeleteAsync(deletedFavorite);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IFavoriteService.GetAllAsync,
        Business.Abstract.IFavoriteService.GetAsync
        ")]
        public async Task<IResult> UpdateAsync(Favorite favorite)
        {
            bool isExists = await _favoriteDal.IsExistAsync(q => q.Id == favorite.Id);
            if (!isExists)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _favoriteDal.UpdateAsync(favorite);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        #endregion

    }
}
