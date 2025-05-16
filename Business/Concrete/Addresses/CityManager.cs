using Business.Abstract.Addresses;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using DataAccess.Abstract.AddressAbstract;
using Entities.Concrete.AddressConcrete;
using System.Linq.Expressions;

namespace Business.Concrete.Addresses
{
    public class CityManager : ICityService
    {
        private readonly ICityDal _cityDal;
        public CityManager(ICityDal cityDal)
        {
            _cityDal = cityDal;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<City>>> GetAllAsync(int index, int size)
        {
            IPaginate<City>? result = await _cityDal.GetListAsync(index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<City>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<City>>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<City>> GetAsync(Expression<Func<City, bool>> filter)
        {
            City? result = await _cityDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<City>(result, Messages.Listed) : new ErrorDataResult<City>(result, Messages.NotListed);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.ICityService.GetAllAsync,
        Business.Abstract.ICityService.GetAsync
        ")]
        public async Task<IResult> UpdateAsync(City city)
        {
            bool isExists = await _cityDal.IsExistAsync(p => p.Id == city.Id);
            if (!isExists)
                return new ErrorResult(Messages.NotFound);

            bool result = await _cityDal.UpdateAsync(city);
            return result ? new SuccessResult(Messages.Updated) : new ErrorResult(Messages.NotUpdated);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.ICityService.GetAllAsync,
        Business.Abstract.ICityService.GetAsync
        ")]
        public async Task<IResult> AddAsync(City city)
        {
            int result = await _cityDal.AddAsync(city);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.ICityService.GetAllAsync,
        Business.Abstract.ICityService.GetAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            City? deletedCity = await _cityDal.GetAsync(p => p.Id == id);
            if (deletedCity == null)
                return new ErrorResult(Messages.NotFound);

            bool result = await _cityDal.DeleteAsync(deletedCity);
            return result ? new SuccessResult(Messages.Deleted) : new ErrorResult(Messages.NotDeleted);
        }
        #endregion
    }
}
