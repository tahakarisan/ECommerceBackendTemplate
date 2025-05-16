using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using System.Linq.Expressions;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {
        private IBrandDal _brandDal;
        private IMapper _mapper;
        public BrandManager(IBrandDal brandDal, IMapper mapper)
        {
            _brandDal = brandDal;
            _mapper = mapper;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<Brand>>> GetAllAsync(int index, int size)
        {
            IPaginate<Brand>? result = await _brandDal.GetListAsync(index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<Brand>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<Brand>>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<Brand>> GetAsync(Expression<Func<Brand, bool>> filter)
        {
            Brand? result = await _brandDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<Brand>(result, Messages.Listed) : new ErrorDataResult<Brand>(result, Messages.NotListed);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.IBrandService.GetAllAsync,
        Business.Abstract.IBrandService.GetAsync
        ")]
        public async Task<IResult> UpdateAsync(Brand brand)
        {
            bool isExists = await _brandDal.IsExistAsync(q => q.Id == brand.Id);
            if (!isExists)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _brandDal.UpdateAsync(brand);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IBrandService.GetAllAsync,
        Business.Abstract.IBrandService.GetAsync
        ")]
        public async Task<IResult> AddAsync(Brand brand)
        {
            int result = await _brandDal.AddAsync(brand);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IBrandService.GetAllAsync,
        Business.Abstract.IBrandService.GetAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            Brand deletedBrand = await _brandDal.GetAsync(q => q.Id == id);
            if (deletedBrand == null)
            {
                return new ErrorResult(Messages.NotFound);
            }
            bool result = await _brandDal.DeleteAsync(deletedBrand);
            return result ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        #endregion
    }
}
