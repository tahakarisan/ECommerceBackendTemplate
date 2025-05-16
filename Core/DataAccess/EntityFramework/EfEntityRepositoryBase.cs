using Core.Entities;
using Core.Extensions;
using Core.Utilities.Dynamic;
using Core.Utilities.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Core.DataAccess.EntityFramework
{
    public class EfEntityRepositoryBase<TEntity, TContext> : IEntityRepository<TEntity>
        where TEntity : class, IEntity, new()
        where TContext : DbContext, new()
    {
        #region Sync
        public virtual TEntity Get(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return context.Set<TEntity>().SingleOrDefault(filter);
            }
        }
        public TEntity Get(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>,
                      IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable();
                if (!enableTracking) queryable = queryable.AsNoTracking();
                if (include != null) queryable = include(queryable);
                return queryable.FirstOrDefault(predicate);
            }
        }
        public virtual List<TEntity> GetAll(Expression<Func<TEntity, bool>>? filter = null)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> datas = filter == null ? context.Set<TEntity>()
                    : context.Set<TEntity>().Where(filter);
                return datas.ToClearCircularList();
            }
        }
        public virtual List<TEntity> GetAllWithInclude(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include, Expression<Func<TEntity, bool>>? filter = null)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable();
                queryable = include(queryable);
                return filter == null ? context.Set<TEntity>().ToList()
                    : context.Set<TEntity>().Where(filter).ToList();
            }
        }

        public IPaginate<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate = null,
                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                     int index = 0, int size = 10,
                                     bool enableTracking = true)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable();
                if (!enableTracking) queryable = queryable.AsNoTracking();
                if (include != null) queryable = include(queryable);
                if (predicate != null) queryable = queryable.Where(predicate);
                if (orderBy != null)
                    return orderBy(queryable).ToPaginate(index, size);
                return queryable.ToPaginate(index, size);
            }
        }
        public IPaginate<TEntity> GetListByDynamic(Dynamic dynamic,
                                                   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>?
                                                       include = null, int index = 0, int size = 10,
                                                   bool enableTracking = true)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable().ToDynamic(dynamic);
                if (!enableTracking) queryable = queryable.AsNoTracking();
                if (include != null) queryable = include(queryable);
                return queryable.ToPaginate(index, size);
            }
        }
        public bool IsExist(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable();
                if (filter == null)
                    return queryable.Any();
                return queryable.Any(filter);
            }
        }
        public int GetTableCount(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable();
                if (filter == null)
                    return queryable.Count();
                return queryable.Count(filter);
            }
        }
        public virtual int Add(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                context.SaveChanges();
                return entity.Id;
            }
        }
        public virtual bool AddRange(List<TEntity> entities)
        {
            using (TContext context = new TContext())
            {
                context.AddRange(entities);
                return context.SaveChanges() > 0;
            }
        }
        public virtual bool Update(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
        }
        public virtual bool Delete(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                return context.SaveChanges() > 0;
            }
        }
        #endregion
        #region Async
        public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                return await context.Set<TEntity>().FirstOrDefaultAsync(filter);
            }
        }
        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>,
                      IIncludableQueryable<TEntity, object>>? include = null, bool enableTracking = true)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable();
                if (!enableTracking) queryable = queryable.AsNoTracking();
                if (include != null) queryable = include(queryable);
                return await queryable.FirstOrDefaultAsync(predicate);
            }
        }
        public virtual async Task<List<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? filter = null)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> datas = filter == null ? context.Set<TEntity>()
                    : context.Set<TEntity>().Where(filter);
                return await datas.ToClearCircularListAsync();
            }
        }
        public virtual async Task<List<TEntity>> GetAllWithIncludeAsync(Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include, Expression<Func<TEntity, bool>>? filter = null)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable();
                queryable = include(queryable);
                return filter == null ? await context.Set<TEntity>().ToListAsync()
                    : await context.Set<TEntity>().Where(filter).ToListAsync();
            }
        }
        public async Task<IPaginate<TEntity>> GetListAsync(Expression<Func<TEntity, bool>>? predicate = null,
                                     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
                                     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
                                     int index = 0, int size = 10,
                                     bool enableTracking = true)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable();
                if (!enableTracking) queryable = queryable.AsNoTracking();
                if (include != null) queryable = include(queryable);
                if (predicate != null) queryable = queryable.Where(predicate);
                if (orderBy != null)
                    return await orderBy(queryable).ToPaginateAsync(index, size);
                return await queryable.ToPaginateAsync(index, size);
            }
        }
        public async Task<IPaginate<TEntity>> GetListByDynamicAsync(Dynamic dynamic,
                                                   Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>?
                                                       include = null, int index = 0, int size = 10,
                                                   bool enableTracking = true)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable().ToDynamic(dynamic);
                if (!enableTracking) queryable = queryable.AsNoTracking();
                if (include != null) queryable = include(queryable);
                return await queryable.ToPaginateAsync(index, size);
            }
        }
        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> filter)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable();
                if (filter == null)
                    return await queryable.AnyAsync();
                return await queryable.AnyAsync(filter);
            }
        }
        public async Task<int> GetTableCountAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            using (TContext context = new TContext())
            {
                IQueryable<TEntity> queryable = context.Set<TEntity>().AsQueryable();
                if (filter == null)
                    return await queryable.CountAsync();
                return await queryable.CountAsync(filter);
            }
        }
        public virtual async Task<int> AddAsync(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> addedEntity = context.Entry(entity);
                addedEntity.State = EntityState.Added;
                await context.SaveChangesAsync();
                return entity.Id;
            }
        }
        public virtual async Task<bool> AddRangeAsync(List<TEntity> entities)
        {
            using (TContext context = new TContext())
            {
                await context.AddRangeAsync(entities);
                return await context.SaveChangesAsync() > 0;
            }
        }
        public virtual async Task<bool> UpdateAsync(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> updatedEntity = context.Entry(entity);
                updatedEntity.State = EntityState.Modified;
                return await context.SaveChangesAsync() > 0;
            }
        }
        public virtual async Task<bool> DeleteAsync(TEntity entity)
        {
            using (TContext context = new TContext())
            {
                Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> deletedEntity = context.Entry(entity);
                deletedEntity.State = EntityState.Deleted;
                return await context.SaveChangesAsync() > 0;
            }
        }
        #endregion
    }
}
