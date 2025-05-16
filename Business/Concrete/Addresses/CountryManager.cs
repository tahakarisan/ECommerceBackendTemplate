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
    public class CountryManager : ICountryService
    {
        private readonly ICountryDal _countryDal;
        public CountryManager(ICountryDal countryDal)
        {
            _countryDal = countryDal;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<Country>>> GetAllAsync(int index, int size)
        {
            IPaginate<Country>? result = await _countryDal.GetListAsync(index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<Country>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<Country>>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<Country>> GetAsync(Expression<Func<Country, bool>> filter)
        {
            Country? result = await _countryDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<Country>(result, Messages.Listed) : new ErrorDataResult<Country>(result, Messages.NotListed);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.ICountryService.GetAllAsync,
        Business.Abstract.ICountryService.GetAsync
        ")]
        public async Task<IResult> UpdateAsync(Country country)
        {
            bool isExists = await _countryDal.IsExistAsync(p => p.Id == country.Id);
            if (!isExists)
                return new ErrorResult(Messages.NotFound);

            bool result = await _countryDal.UpdateAsync(country);
            return result ? new SuccessResult(Messages.Updated) : new ErrorResult(Messages.NotUpdated);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.ICountryService.GetAllAsync,
        Business.Abstract.ICountryService.GetAsync
        ")]
        public async Task<IResult> AddAsync(Country country)
        {
            int result = await _countryDal.AddAsync(country);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.ICountryService.GetAllAsync,
        Business.Abstract.ICountryService.GetAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            Country? deletedCountry = await _countryDal.GetAsync(p => p.Id == id);
            if (deletedCountry == null)
                return new ErrorResult(Messages.NotFound);

            bool result = await _countryDal.DeleteAsync(deletedCountry);
            return result ? new SuccessResult(Messages.Deleted) : new ErrorResult(Messages.NotDeleted);
        }
        #endregion
    }
}
