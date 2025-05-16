using Core.Utilities.Paging;
using Core.Utilities.Results;
using Entities.Concrete.AddressConcrete;
using Entities.DTOs.Addresses;
using System.Linq.Expressions;

namespace Business.Abstract.Addresses
{
    public interface IAddressService
    {
        #region Queries
        Task<IDataResult<IPaginate<Address>>> GetAllAsync(int index, int size);
        Task<IDataResult<Address>> GetAsync(Expression<Func<Address, bool>> filter);
        Task<IDataResult<Address>> GetByUserIdAsync(int userId);
        Task<IDataResult<IPaginate<Address>>> GetAllByUserIdAsync(int index, int size, int userId);
        #endregion
        #region Commands
        Task<IResult> AddAsync(AddAddressDto addAddressDto);
        Task<IResult> UpdateAsync(Address address);
        Task<IResult> DeleteAsync(int id);
        #endregion
    }
}
