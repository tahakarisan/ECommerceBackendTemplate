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
    public class DistrictManager : IDistrictService
    {
        private readonly IDistrictDal _districtDal;
        public DistrictManager(IDistrictDal districtDal)
        {
            _districtDal = districtDal;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<District>>> GetAllAsync(int index, int size)
        {
            IPaginate<District>? result = await _districtDal.GetListAsync(index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<District>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<District>>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<District>> GetAsync(Expression<Func<District, bool>> filter)
        {
            District? result = await _districtDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<District>(result, Messages.Listed) : new ErrorDataResult<District>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<District>>> GetByCountyIdAsync(int index, int size, string countyId)
        {
            IPaginate<District>? result = await _districtDal.GetListAsync(index: index, size: size, predicate: p => p.CountyId == countyId);
            return result != null ? new SuccessDataResult<IPaginate<District>>(result) : new ErrorDataResult<IPaginate<District>>(result);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.IDistrictService.GetAllAsync,
        Business.Abstract.IDistrictService.GetAsync,
        Business.Abstract.IDistrictService.GetByCountyIdAsync
        ")]
        public async Task<IResult> UpdateAsync(District district)
        {
            bool isExists = await _districtDal.IsExistAsync(p => p.Id == district.Id);
            if (!isExists)
                return new ErrorResult(Messages.NotFound);

            bool result = await _districtDal.UpdateAsync(district);
            return result ? new SuccessResult(Messages.Updated) : new ErrorResult(Messages.NotUpdated);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IDistrictService.GetAllAsync,
        Business.Abstract.IDistrictService.GetAsync,
        Business.Abstract.IDistrictService.GetByCountyIdAsync
        ")]
        public async Task<IResult> AddAsync(District district)
        {
            int result = await _districtDal.AddAsync(district);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IDistrictService.GetAllAsync,
        Business.Abstract.IDistrictService.GetAsync,
        Business.Abstract.IDistrictService.GetByCountyIdAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            District? deletedDistrict = await _districtDal.GetAsync(p => p.Id == id);
            if (deletedDistrict == null)
                return new ErrorResult(Messages.NotFound);

            bool result = await _districtDal.DeleteAsync(deletedDistrict);
            return result ? new SuccessResult(Messages.Deleted) : new ErrorResult(Messages.NotDeleted);
        }
        #endregion
    }
}
