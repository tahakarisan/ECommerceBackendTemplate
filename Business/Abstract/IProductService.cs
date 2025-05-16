using Core.Utilities.Dynamic;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Products;
using System.Linq.Expressions;

namespace Business.Abstract
{
    public interface IProductService
    {
        #region Queries
        Task<IDataResult<Product>> GetAsync(Expression<Func<Product, bool>> filter);
        Task<IDataResult<IPaginate<Product>>> GetAllAsync(int index, int size);
        Task<IDataResult<Product>> GetMostExpensiveProductAsync();
        Task<IResult> AddWithImageAsync(AddProductWithImageDto addProductWithImageDto);
        Task<IDataResult<IPaginate<Product>>> GetListDynamicAsync(int index, int size, Dynamic dynamic);
        Task<IDataResult<IPaginate<ProductDetailDto>>> GetProductDetailDtoAsync(int index, int size);
        Task<int> GetProductsCountFromDalAsync();
        Task<IDataResult<IPaginate<ProductDetailDto>>> GetRelatedProductsByProductIdAsync(int productId, int index = 0, int size = 20);
        Task<IDataResult<IPaginate<ProductDetailDto>>> GetRelatedProductsByCategoryNameAsync(string categoryName, int index = 0, int size = 20);
        Task<List<ProductDetailDto>> GetPopularProductsAsync(int index = 0, int size = 20);
        Task<IDataResult<ProductDetailDto>> GetProductDetailByIdAsync(int id);
        #endregion
        #region Commands
        Task<IResult> AddAsync(AddProductDto addProductDto);
        Task<IResult> AddRangeAsync(List<Product> products);
        Task<IResult> UpdateAsync(Product product);
        Task<IResult> DeleteAsync(int id);
        #endregion     
    }
}
