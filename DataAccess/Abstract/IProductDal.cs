using Core.DataAccess;
using Core.Utilities.Paging;
using Entities.Concrete;
using Entities.DTOs.Products;

namespace DataAccess.Abstract
{
    public interface IProductDal : IEntityRepository<Product>
    {
        #region Queries
        Task<Product> GetMostExpensiveProductAsync();
        Task<IPaginate<ProductDetailDto>> GetProductDetailDtoAsync(int index, int size);
        Task<IPaginate<ProductDetailDto>> GetProductDetailDtoByCategoryIdAsync(int categoryId, int index, int size);
        Task<int> GetProductsCountFromDalAsync();
        Task<List<ProductDetailDto>> GetPopularProductsAsync(int index = 0, int size = 20);
        Task<ProductDetailDto> GetProductDetailByIdAsync(int id);
        #endregion
        #region Commands
        #endregion
    }
}
