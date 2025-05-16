using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfCategoryDal : EfEntityRepositoryBase<Category, ECommerceContext>, ICategoryDal
    {
        public async Task<bool> CategoryIsExistAsync(string name)
        {
            using (ECommerceContext context = new ECommerceContext())
            {
                return await context.Categories.AnyAsync(p => p.Name == name);
            }
        }
        #region Queries
        public async Task<List<Category>> GetAllParentCategoryAsync()
        {
            using (ECommerceContext context = new ECommerceContext())
            {
                IQueryable<Category> queryable = context.Set<Category>().AsQueryable();
                return await queryable.Where(p => p.SubCategories.Count() > 0).ToListAsync();
            }
        }
        public async Task<int> GetAllParentCategoryCountAsync()
        {
            using (ECommerceContext context = new ECommerceContext())
            {
                IQueryable<Category> queryable = context.Set<Category>().AsQueryable();
                return await queryable.CountAsync(p => p.SubCategories.Count() > 0);
            }
        }
        public async Task<List<Category>> GetChildCategoriesByCategoryIdAsync(int categoryId)
        {
            using (ECommerceContext context = new ECommerceContext())
            {
                IQueryable<Category> queryable = context.Set<Category>().AsQueryable();
                bool categoryExists = await queryable.AnyAsync(p => p.Id == categoryId);
                if (categoryExists)
                {
                    return await queryable.Where(q => q.ParentCategoryId == categoryId).ToListAsync();
                }
                return new List<Category>();
            }
        }
        #endregion
    }
}
