using AutoMapper;
using Business.Abstract.Addresses;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using DataAccess.Abstract.AddressAbstract;
using Entities.Concrete.AddressConcrete;
using Entities.DTOs.Addresses;
using System.Linq.Expressions;

namespace Business.Concrete.Addresses
{
    public class AddressManager : IAddressService
    {
        private readonly IAddressDal _addressDal;
        private readonly IMapper _mapper;
        public AddressManager(IAddressDal addressDal, IMapper mapper)
        {
            _addressDal = addressDal;
            _mapper = mapper;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<Address>>> GetAllAsync(int index, int size)
        {
            IPaginate<Address>? result = await _addressDal.GetListAsync(index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<Address>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<Address>>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<Address>>> GetAllByUserIdAsync(int index, int size, int userId)
        {
            IPaginate<Address> result = await _addressDal.GetListAsync(index: index, size: size, predicate: p => p.UserId == userId);
            return result.Count != 0 ? new SuccessDataResult<IPaginate<Address>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<Address>>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<Address>> GetAsync(Expression<Func<Address, bool>> filter)
        {
            Address? result = await _addressDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<Address>(result, Messages.Listed) : new ErrorDataResult<Address>(result, Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<Address>> GetByUserIdAsync(int userId)
        {
            Address? result = await _addressDal.GetAsync(p => p.UserId == userId);
            return result != null ? new SuccessDataResult<Address>(result, Messages.Listed) : new ErrorDataResult<Address>(result, Messages.NotListed);
        }
        #endregion
        #region Commands      
        [CacheRemoveAspect(@"
        Business.Abstract.IAddressService.GetAllAsync,
        Business.Abstract.IAddressService.GetAllByUserIdAsync,
        Business.Abstract.IAddressService.GetAsync,
        Business.Abstract.IAddressService.GetByUserIdAsync
        ")]
        public async Task<IResult> UpdateAsync(Address address)
        {
            bool isExists = await _addressDal.IsExistAsync(p => p.Id == address.Id);
            if (!isExists)
                return new ErrorResult(Messages.NotFound);

            bool result = await _addressDal.UpdateAsync(address);
            return result ? new SuccessResult(Messages.Updated) : new ErrorResult(Messages.NotUpdated);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IAddressService.GetAllAsync,
        Business.Abstract.IAddressService.GetAllByUserIdAsync,
        Business.Abstract.IAddressService.GetAsync,
        Business.Abstract.IAddressService.GetByUserIdAsync
        ")]
        public async Task<IResult> AddAsync(AddAddressDto addAddressDto)
        {
            Address address = _mapper.Map<Address>(addAddressDto);
            int result = await _addressDal.AddAsync(address);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IAddressService.GetAllAsync,
        Business.Abstract.IAddressService.GetAllByUserIdAsync,
        Business.Abstract.IAddressService.GetAsync,
        Business.Abstract.IAddressService.GetByUserIdAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            Address? deletedAddress = await _addressDal.GetAsync(p => p.Id == id);
            if (deletedAddress == null)
                return new ErrorResult(Messages.NotFound);

            bool result = await _addressDal.DeleteAsync(deletedAddress);
            return result ? new SuccessResult(Messages.Deleted) : new ErrorResult(Messages.NotDeleted);
        }
        #endregion
    }
}
