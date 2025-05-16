using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Products;
using System.Linq.Expressions;

namespace Business.Abstract
{
    public interface IProductImageService
    {
        public Task<IResult> AddAsync(ProductImage productImage);
        public Task<IResult> UpdateAsync(ProductImage productImage);
        public Task<IResult> DeleteAsync(int id);
        public Task<IDataResult<ProductImage>> GetAsync(Expression<Func<ProductImage, bool>> filter);
        public Task<IDataResult<List<ProductImage>>> GetAllAsync();
        public Task<IResult> AddProductImageByProductIdAsync(AddProductImageDto addProductImageDto);
        public Task<IResult> AddDefaultProductImageByProductIdAsync(int productId);
    }
}
