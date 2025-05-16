using Core.Utilities.Paging;
using Core.Utilities.Results;
using Entities.Concrete.AddressConcrete;
using System.Linq.Expressions;

namespace Business.Abstract.Addresses
{
    public interface IDistrictService
    {
        #region Queries
        Task<IDataResult<IPaginate<District>>> GetAllAsync(int index, int size);
        Task<IDataResult<IPaginate<District>>> GetByCountyIdAsync(int index, int size, string countyId);
        Task<IDataResult<District>> GetAsync(Expression<Func<District, bool>> filter);
        #endregion
        #region Commands
        Task<IResult> AddAsync(District district);
        Task<IResult> UpdateAsync(District district);
        Task<IResult> DeleteAsync(int id);
        #endregion
    }
}
