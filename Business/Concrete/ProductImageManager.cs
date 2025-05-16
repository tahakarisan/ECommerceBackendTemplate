using Business.Abstract;
using Business.Constants;
using Business.Utilities.File;
using Core.Aspects.Autofac.Caching;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Products;
using System.Linq.Expressions;

namespace Business.Concrete
{
    public class ProductImageManager : IProductImageService
    {
        private readonly IProductImageDal _productImageDal;
        private readonly IProductImageUploadService _productImageUploadService;

        public ProductImageManager(IProductImageDal productImageDal, IProductImageUploadService productImageUploadService)
        {
            _productImageDal = productImageDal;
            _productImageUploadService = productImageUploadService;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<ProductImage>> GetAsync(Expression<Func<ProductImage, bool>> filter)
        {
            ProductImage result = await _productImageDal.GetAsync(filter);
            return result != null ? new SuccessDataResult<ProductImage>(result, Messages.Listed) : new ErrorDataResult<ProductImage>(Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<List<ProductImage>>> GetAllAsync()
        {
            List<ProductImage> result = await _productImageDal.GetAllAsync();
            return result != null ? new SuccessDataResult<List<ProductImage>>(result, Messages.Listed) : new ErrorDataResult<List<ProductImage>>(Messages.NotListed);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.IProductImageService.GetAllAsync,
        Business.Abstract.IProductImageService.GetAsync
        ")]
        public async Task<IResult> AddAsync(ProductImage productImage)
        {
            int result = await _productImageDal.AddAsync(productImage);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IProductImageService.GetAllAsync,
        Business.Abstract.IProductImageService.GetAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            ProductImage productImage = await _productImageDal.GetAsync(p => p.Id == id);
            if (productImage == null)
                return new ErrorResult(Messages.NotFound);
            bool result = await _productImageDal.DeleteAsync(productImage);
            return result ? new SuccessResult(Messages.Deleted) : new ErrorResult(Messages.NotDeleted);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IProductImageService.GetAllAsync,
        Business.Abstract.IProductImageService.GetAsync
        ")]
        public async Task<IResult> UpdateAsync(ProductImage productImage)
        {
            bool isExists = await _productImageDal.IsExistAsync(p => p.Id == productImage.Id);
            if (!isExists)
                return new ErrorResult(Messages.NotFound);
            bool result = await _productImageDal.UpdateAsync(productImage);
            return result ? new SuccessResult(Messages.Updated) : new ErrorResult(Messages.NotUpdated);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IProductImageService.GetAllAsync,
        Business.Abstract.IProductImageService.GetAsync
        ")]
        public async Task<IResult> AddProductImageByProductIdAsync(AddProductImageDto addProductImageDto)
        {
            int uploadingResult = 0;
            for (int i = 0; i < addProductImageDto.IFormFiles.Count(); i++)
            {
                (string, bool) imageResult = _productImageUploadService.AddImage(addProductImageDto.IFormFiles[i]);
                if (imageResult.Item2)
                {
                    uploadingResult = await _productImageDal.AddAsync(new ProductImage { ImageUrl = imageResult.Item1, Name = imageResult.Item1, ProductId = addProductImageDto.ProductId });
                }
            }
            return uploadingResult > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.Error);
        }
        //Todo: image url config den alınabilir veya başka bir çözüm olabilir. değişecek.
        [CacheRemoveAspect(@"
        Business.Abstract.IProductImageService.GetAllAsync,
        Business.Abstract.IProductImageService.GetAsync
        ")]
        public async Task<IResult> AddDefaultProductImageByProductIdAsync(int productId)
        {
            int result = await _productImageDal.AddAsync(new ProductImage { ImageUrl = "\\ProductImages\\defaultFile.jpg", Name = "defaultFile.jpg", ProductId = productId });
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        #endregion
    }
}
