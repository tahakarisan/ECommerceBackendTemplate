using AutoMapper;
using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.Utilities.File;
using Business.ValidationRules.FluentValidations;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Validation;
using Core.Utilities.Dynamic;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Products;
using System.Linq.Expressions;

namespace Business.Concrete
{
    public class ProductManager : IProductService
    {
        private readonly IProductDal _productDal;
        private readonly IProductImageService _productImageService;
        private readonly IProductImageUploadService _productImageUploadService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public ProductManager(IProductDal productDal, IProductImageUploadService productImageUploadService, IProductImageService productImageService, IMapper mapper, ICategoryService categoryService)
        {
            _productDal = productDal;
            _productImageUploadService = productImageUploadService;
            _productImageService = productImageService;
            _mapper = mapper;
            _categoryService = categoryService;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<Product>> GetAsync(Expression<Func<Product, bool>> filter)
        {
            Product product = await _productDal.GetAsync(filter);
            return product != null ? new SuccessDataResult<Product>(product, "") : new ErrorDataResult<Product>("Hata Oluştu");
        }
        [CacheAspect(60)]
        [SecuredOperation("getall")]
        public async Task<IDataResult<IPaginate<Product>>> GetAllAsync(int index = 0, int size = 50)
        {
            IPaginate<Product> result = await _productDal.GetListAsync(index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<Product>>(result, "Listelendi") : new ErrorDataResult<IPaginate<Product>>("Hata Oluştu");
        }
        [CacheAspect(60)]
        public async Task<IDataResult<Product>> GetMostExpensiveProductAsync()
        {
            Product result = await _productDal.GetMostExpensiveProductAsync();
            return result != null ? new SuccessDataResult<Product>(result, "") : new ErrorDataResult<Product>("Hata Oluştu");
        }
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<Product>>> GetListDynamicAsync(int index, int size, Dynamic dynamic)
        {
            IPaginate<Product> result = await _productDal.GetListByDynamicAsync(size: size, index: index, dynamic: dynamic);
            return result != null ? new SuccessDataResult<IPaginate<Product>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<Product>>(Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<ProductDetailDto>>> GetProductDetailDtoAsync(int index, int size)
        {
            IPaginate<ProductDetailDto> result = await _productDal.GetProductDetailDtoAsync(index, size);
            return result != null ? new SuccessDataResult<IPaginate<ProductDetailDto>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<ProductDetailDto>>(Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<ProductDetailDto>>> GetProductDetailDtoByCategoryIdAsync(int categoryId, int index, int size)
        {
            IPaginate<ProductDetailDto> result = await _productDal.GetProductDetailDtoByCategoryIdAsync(categoryId, index, size);
            return result != null ? new SuccessDataResult<IPaginate<ProductDetailDto>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<ProductDetailDto>>(Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<ProductDetailDto>>> GetRelatedProductsByProductIdAsync(int productId, int index = 0, int size = 20)
        {
            Product product = await _productDal.GetAsync(p => p.Id == productId);
            if (product == null)
                return new ErrorDataResult<IPaginate<ProductDetailDto>>(Messages.Error);

            IPaginate<ProductDetailDto> result = await _productDal.GetProductDetailDtoByCategoryIdAsync(categoryId: product.CategoryId, index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<ProductDetailDto>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<ProductDetailDto>>(Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<IPaginate<ProductDetailDto>>> GetRelatedProductsByCategoryNameAsync(string categoryName, int index = 0, int size = 20)
        {
            IDataResult<Entities.DTOs.Categories.CategoryDto> categoryResult = await _categoryService.GetAsync(q => q.Name == categoryName);
            if (categoryResult.Success || categoryResult.Data == null)
                return new ErrorDataResult<IPaginate<ProductDetailDto>>(Messages.Error);

            IPaginate<ProductDetailDto> result = await _productDal.GetProductDetailDtoByCategoryIdAsync(categoryId: categoryResult.Data.Id, index: index, size: size);
            return result != null ? new SuccessDataResult<IPaginate<ProductDetailDto>>(result, Messages.Listed) : new ErrorDataResult<IPaginate<ProductDetailDto>>(Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<List<ProductDetailDto>> GetPopularProductsAsync(int index = 0, int size = 20)
        {
            return await _productDal.GetPopularProductsAsync(index: index, size: size);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<ProductDetailDto>> GetProductDetailByIdAsync(int id)
        {
            ProductDetailDto result = await _productDal.GetProductDetailByIdAsync(id);
            return result != null ? new SuccessDataResult<ProductDetailDto>(result) : new ErrorDataResult<ProductDetailDto>(Messages.NotFound);
        }
        #endregion
        #region Commands
        [ValidationAspect(typeof(ProductValidator))]
        [CacheRemoveAspect(@"
        Business.Abstract.IProductService.GetAsync,
        Business.Abstract.IProductService.GetAllAsync,
        Business.Abstract.IProductService.GetMostExpensiveProductAsync,
        Business.Abstract.IProductService.GetListDynamicAsync,
        Business.Abstract.IProductService.GetProductDetailDtoAsync,
        Business.Abstract.IProductService.GetProductDetailDtoByCategoryIdAsync,
        Business.Abstract.IProductService.GetRelatedProductsByProductIdAsync,
        Business.Abstract.IProductService.GetRelatedProductsByCategoryNameAsync,
        Business.Abstract.IProductService.GetPopularProductsAsync,
        Business.Abstract.IProductService.GetProductDetailByIdAsync,
        Business.Abstract.IProductService.GetProductsCountFromDalAsync
        ")]
        public async Task<IResult> AddAsync(AddProductDto addProductDto)
        {
            Random rnd = new Random();
            Product product = _mapper.Map<Product>(addProductDto);
            int id = 0;
            //for (int i = 0; i <= 999; i++)
            {
                //product.Id = 0;
                //product.CategoryId = "1-2";
                //IResult bussinessRules = BusinessRules.Run(categoryIsExist(product.CategoryId));
                //if (bussinessRules != null)
                //{
                //    return bussinessRules;
                //}c
                id = await _productDal.AddAsync(product);
                if (id > 0)
                    await _productImageService.AddDefaultProductImageByProductIdAsync(id);
            }
            return id > 0 ? new SuccessResult("eklendi") : new ErrorResult("eklenemedi hata oluştu");
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IProductService.GetAsync,
        Business.Abstract.IProductService.GetAllAsync,
        Business.Abstract.IProductService.GetMostExpensiveProductAsync,
        Business.Abstract.IProductService.GetListDynamicAsync,
        Business.Abstract.IProductService.GetProductDetailDtoAsync,
        Business.Abstract.IProductService.GetProductDetailDtoByCategoryIdAsync,
        Business.Abstract.IProductService.GetRelatedProductsByProductIdAsync,
        Business.Abstract.IProductService.GetRelatedProductsByCategoryNameAsync,
        Business.Abstract.IProductService.GetPopularProductsAsync,
        Business.Abstract.IProductService.GetProductDetailByIdAsync,
        Business.Abstract.IProductService.GetProductsCountFromDalAsync
        ")]
        public async Task<IResult> AddRangeAsync(List<Product> products)
        {
            bool result = await _productDal.AddRangeAsync(products);
            return result ? new SuccessResult("eklendi") : new ErrorResult("eklenemedi hata oluştu");
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IProductService.GetAsync,
        Business.Abstract.IProductService.GetAllAsync,
        Business.Abstract.IProductService.GetMostExpensiveProductAsync,
        Business.Abstract.IProductService.GetListDynamicAsync,
        Business.Abstract.IProductService.GetProductDetailDtoAsync,
        Business.Abstract.IProductService.GetProductDetailDtoByCategoryIdAsync,
        Business.Abstract.IProductService.GetRelatedProductsByProductIdAsync,
        Business.Abstract.IProductService.GetRelatedProductsByCategoryNameAsync,
        Business.Abstract.IProductService.GetPopularProductsAsync,
        Business.Abstract.IProductService.GetProductDetailByIdAsync,
        Business.Abstract.IProductService.GetProductsCountFromDalAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            bool result = false;
            Product product = await _productDal.GetAsync(p => p.Id == id);
            if (product != null)
            {
                result = await _productDal.DeleteAsync(product);
            }
            return result ? new SuccessResult("Silindi") : new ErrorResult("Silinemedi hata oluştu");
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IProductService.GetAsync,
        Business.Abstract.IProductService.GetAllAsync,
        Business.Abstract.IProductService.GetMostExpensiveProductAsync,
        Business.Abstract.IProductService.GetListDynamicAsync,
        Business.Abstract.IProductService.GetProductDetailDtoAsync,
        Business.Abstract.IProductService.GetProductDetailDtoByCategoryIdAsync,
        Business.Abstract.IProductService.GetRelatedProductsByProductIdAsync,
        Business.Abstract.IProductService.GetRelatedProductsByCategoryNameAsync,
        Business.Abstract.IProductService.GetPopularProductsAsync,
        Business.Abstract.IProductService.GetProductDetailByIdAsync,
        Business.Abstract.IProductService.GetProductsCountFromDalAsync
        ")]
        public async Task<IResult> UpdateAsync(Product product)
        {
            bool result = false;
            bool isExists = await _productDal.IsExistAsync(p => p.Id == product.Id);
            if (isExists)
            {
                result = await _productDal.UpdateAsync(product);
            }
            return result ? new SuccessResult("Güncellendi") : new ErrorResult("Güncellenemedi hata oluştu");
        }
        [CacheRemoveAspect(@"
        Business.Abstract.IProductService.GetAsync,
        Business.Abstract.IProductService.GetAllAsync,
        Business.Abstract.IProductService.GetMostExpensiveProductAsync,
        Business.Abstract.IProductService.GetListDynamicAsync,
        Business.Abstract.IProductService.GetProductDetailDtoAsync,
        Business.Abstract.IProductService.GetProductDetailDtoByCategoryIdAsync,
        Business.Abstract.IProductService.GetRelatedProductsByProductIdAsync,
        Business.Abstract.IProductService.GetRelatedProductsByCategoryNameAsync,
        Business.Abstract.IProductService.GetPopularProductsAsync,
        Business.Abstract.IProductService.GetProductDetailByIdAsync,
        Business.Abstract.IProductService.GetProductsCountFromDalAsync
        ")]
        public async Task<IResult> AddWithImageAsync(AddProductWithImageDto addProductWithImageDto)
        {
            Product product = _mapper.Map<Product>(addProductWithImageDto);
            int id = await _productDal.AddAsync(product);
            if (id > 0)
            {
                for (int i = 0; i < addProductWithImageDto.IFormFiles.Count(); i++)
                {
                    (string, bool) imageResult = _productImageUploadService.AddImage(addProductWithImageDto.IFormFiles[i]);
                    if (imageResult.Item2)
                    {
                        IResult uploadingResult = await _productImageService.AddAsync(new ProductImage { ImageUrl = imageResult.Item1, Name = product.Name, ProductId = product.Id }); ;
                        //if (!uploadingResult.Success)
                        //{
                        //    return new ErrorResult(Messages.Error);
                        //}
                    }
                }
                return new SuccessResult(Messages.Added);
            }
            else
            {
                return new ErrorResult(Messages.NotAdded);
            }
        }
        #endregion
        #region Rules
        [CacheAspect(60)]
        public async Task<int> GetProductsCountFromDalAsync()
        {
            int totalCount = await _productDal.GetProductsCountFromDalAsync();//46 ms avg.
            return totalCount;
        }
        #endregion
    }
}
