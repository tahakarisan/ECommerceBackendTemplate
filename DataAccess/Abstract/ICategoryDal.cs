using Core.DataAccess;
using Entities.Concrete;

namespace DataAccess.Abstract
{
    public interface ICategoryDal : IEntityRepository<Category>
    {
        #region Queries
        Task<List<Category>> GetAllParentCategoryAsync();
        Task<int> GetAllParentCategoryCountAsync();
        Task<List<Category>> GetChildCategoriesByCategoryIdAsync(int categoryId);
        Task<bool> CategoryIsExistAsync(string name);
        #endregion
    }
}
