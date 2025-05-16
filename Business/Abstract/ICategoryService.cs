using Core.Utilities.Paging;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Categories;
using Entities.DTOs.Categories.TrendyolDtos;
using System.Linq.Expressions;

namespace Business.Abstract
{
    public interface ICategoryService
    {
        #region Queries
        Task<IDataResult<CategoryDto>> GetAsync(Expression<Func<Category, bool>> filter);
        Task<IDataResult<Paginate<CategoryDto>>> GetAllAsync(int index, int size);
        Task<IDataResult<CategoryDto>> GetByCategoryIdAsync(int categoryId);
        Task<IDataResult<List<CategoryDto>>> GetChildCategoriesByCategoryIdAsync(int categoryId);
        #endregion
        #region Commands
        Task<IDataResult<CategoryDto>> AddWithDtoAsync(AddCategoryDto addCategoryDto);
        Task<IResult> AddAsync(Category category);
        Task<IResult> UpdateAsync(Category category);
        Task<IResult> DeleteAsync(int id);
        Task AddCategoryAsync(TrendyolCategoryDto category);
        #endregion
    }
}
