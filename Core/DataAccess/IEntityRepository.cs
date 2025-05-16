using Core.Utilities.Dynamic;
using Core.Utilities.Paging;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.DataAccess
{
    public interface IEntityRepository<T> where T : class
    {
        #region Sync
        T Get(Expression<Func<T, bool>> filter);
        T Get(Expression<Func<T, bool>> predicate, Func<IQueryable<T>,
        IIncludableQueryable<T, object>>? include = null, bool enableTracking = true);
        List<T> GetAll(Expression<Func<T, bool>>? filter = null);
        List<T> GetAllWithInclude(Func<IQueryable<T>, IIncludableQueryable<T, object>> include, Expression<Func<T, bool>>? filter = null);
        IPaginate<T> GetList(Expression<Func<T, bool>>? predicate = null,
                             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                             int index = 0, int size = 10,
                             bool enableTracking = true);
        IPaginate<T> GetListByDynamic(Dynamic dynamic,
                                      Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                      int index = 0, int size = 10, bool enableTracking = true);
        bool IsExist(Expression<Func<T, bool>> filter);
        int GetTableCount(Expression<Func<T, bool>> filter = null);
        int Add(T entity);
        bool AddRange(List<T> entities);
        bool Update(T entity);
        bool Delete(T entity);
        #endregion
        #region Async
        Task<T> GetAsync(Expression<Func<T, bool>> filter);
        Task<T> GetAsync(Expression<Func<T, bool>> predicate, Func<IQueryable<T>,
        IIncludableQueryable<T, object>>? include = null, bool enableTracking = true);
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<List<T>> GetAllWithIncludeAsync(Func<IQueryable<T>, IIncludableQueryable<T, object>> include, Expression<Func<T, bool>>? filter = null);
        Task<IPaginate<T>> GetListAsync(Expression<Func<T, bool>>? predicate = null,
                             Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
                             Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                             int index = 0, int size = 10,
                             bool enableTracking = true);
        Task<IPaginate<T>> GetListByDynamicAsync(Dynamic dynamic,
                                      Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
                                      int index = 0, int size = 10, bool enableTracking = true);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> filter);
        Task<int> GetTableCountAsync(Expression<Func<T, bool>> filter = null);
        Task<int> AddAsync(T entity);
        Task<bool> AddRangeAsync(List<T> entities);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        #endregion
    }
}
