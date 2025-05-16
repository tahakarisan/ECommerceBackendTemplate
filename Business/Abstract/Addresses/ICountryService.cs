using Core.Utilities.Paging;
using Core.Utilities.Results;
using Entities.Concrete.AddressConcrete;
using System.Linq.Expressions;

namespace Business.Abstract.Addresses
{
    public interface ICountryService
    {
        #region Queries
        Task<IDataResult<IPaginate<Country>>> GetAllAsync(int index, int size);
        Task<IDataResult<Country>> GetAsync(Expression<Func<Country, bool>> filter);
        #endregion
        #region Commands
        Task<IResult> AddAsync(Country country);
        Task<IResult> UpdateAsync(Country country);
        Task<IResult> DeleteAsync(int id);
        #endregion
    }
}
