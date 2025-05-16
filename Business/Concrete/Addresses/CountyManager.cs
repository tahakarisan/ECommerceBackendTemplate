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
    public class CountyManager : ICountyService
    {
        private readonly ICountyDal _countyDal;
        public CountyManager(ICountyDal countyDal)
        {
            _countyDal = countyDal;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<County>>> GetAllAsync(int index, int size)
        {
            IPaginate<County> result = await _countyDal.GetListAsync(index: index, size: size);
            return new SuccessDataResult<IPaginate<County>>(result, Messages.Listed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<County>> GetAsync(Expression<Func<County, bool>> filter)
        {
            County? result = await _countyDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<County>(result, Messages.Listed) : new ErrorDataResult<County>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<County>>> GetCountyByCityIdAsync(int index, int size, int cityId)
        {
            IPaginate<County> resut = await _countyDal.GetListAsync(index: index, size: size, predicate: p => p.CityId == cityId);
            return resut.Count > 0 ? new SuccessDataResult<IPaginate<County>>(resut, Messages.Listed) : new ErrorDataResult<IPaginate<County>>(resut, Messages.NotListed);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.ICountyService.GetAllAsync,
        Business.Abstract.ICountyService.GetAsync,
        Business.Abstract.ICountyService.GetCountyByCityIdAsync
        ")]
        public async Task<IResult> UpdateAsync(County county)
        {
            bool isExists = await _countyDal.IsExistAsync(p => p.Id == county.Id);
            if (!isExists)
                return new ErrorResult(Messages.NotFound);

            bool result = await _countyDal.UpdateAsync(county);
            return result ? new SuccessResult(Messages.Updated) : new ErrorResult(Messages.NotUpdated);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.ICountyService.GetAllAsync,
        Business.Abstract.ICountyService.GetAsync,
        Business.Abstract.ICountyService.GetCountyByCityIdAsync
        ")]
        public async Task<IResult> AddAsync(County county)
        {
            int result = await _countyDal.AddAsync(county);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.ICountyService.GetAllAsync,
        Business.Abstract.ICountyService.GetAsync,
        Business.Abstract.ICountyService.GetCountyByCityIdAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            County? deletedCounty = await _countyDal.GetAsync(p => p.Id == id);
            if (deletedCounty == null)
                return new ErrorResult(Messages.NotFound);

            bool result = await _countyDal.DeleteAsync(deletedCounty);
            return result ? new SuccessResult(Messages.Deleted) : new ErrorResult(Messages.NotDeleted);
        }
        #endregion
    }
}
