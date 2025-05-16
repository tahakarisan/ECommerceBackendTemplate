using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Core.Aspects.Autofac.Caching;
using Core.Extensions;
using Core.Utilities.Paging;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs.Categories;
using Entities.DTOs.Categories.TrendyolDtos;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        private readonly ICategoryDal _categoryDal;
        private readonly IMapper _mapper;
        public CategoryManager(ICategoryDal categoryDal, IMapper mapper)
        {
            _categoryDal = categoryDal;
            _mapper = mapper;
        }
        #region Queries
        [CacheAspect(60)]
        public async Task<IDataResult<CategoryDto>> GetAsync(Expression<Func<Category, bool>> filter)
        {
            CategoryDto result = _mapper.Map<CategoryDto>(await _categoryDal.GetAsync(filter));
            return result != null ? new SuccessDataResult<CategoryDto>(result, "") : new ErrorDataResult<CategoryDto>("Hata Oluştu");
        }
        [CacheAspect(60)]
        public async Task<IDataResult<Paginate<CategoryDto>>> GetAllAsync(int index, int size)
        {
            IPaginate<Category> result = await _categoryDal.GetListAsync(index: index, size: size);
            IDataResult<Paginate<CategoryDto>> dataResult = result != null ? new SuccessDataResult<Paginate<CategoryDto>>(result.ToMappedPaginate<Category, CategoryDto>(), Messages.Listed) : new ErrorDataResult<Paginate<CategoryDto>>(Messages.NotListed);
            return dataResult;
        }
        [CacheAspect(60)]
        public async Task<IDataResult<CategoryDto>> GetByCategoryIdAsync(int categoryId)
        {
            Category? result = await _categoryDal.GetAsync(p => p.Id == categoryId);
            return result != null ? new SuccessDataResult<CategoryDto>(_mapper.Map<CategoryDto>(result), Messages.Listed) : new ErrorDataResult<CategoryDto>(Messages.NotListed);
        }
        [CacheAspect(60)]
        public async Task<IDataResult<List<CategoryDto>>> GetChildCategoriesByCategoryIdAsync(int categoryId)
        {
            List<Category>? result = await _categoryDal.GetChildCategoriesByCategoryIdAsync(categoryId);
            return result != null ? new SuccessDataResult<List<CategoryDto>>(_mapper.Map<List<CategoryDto>>(result), Messages.Listed) : new ErrorDataResult<List<CategoryDto>>(_mapper.Map<List<CategoryDto>>(result), Messages.Error);
        }
        #endregion
        #region Commands
        [CacheRemoveAspect(@"
        Business.Abstract.ICategoryService.GetAsync,
        Business.Abstract.ICategoryService.GetAllAsync,
        Business.Abstract.ICategoryService.GetByCategoryIdAsync,
        Business.Abstract.ICategoryService.GetChildCategoriesByCategoryIdAsync
        ")]
        public async Task<IDataResult<CategoryDto>> AddWithDtoAsync(AddCategoryDto addCategoryDto)
        {
            if (addCategoryDto.ParentCategoryId != null)
            {
                if (!await _categoryDal.IsExistAsync(p => p.Id == addCategoryDto.ParentCategoryId))
                {
                    return new ErrorDataResult<CategoryDto>(Messages.ParentCategoryNotFound);
                }
            }

            Category newCategory = new Category
            {
                Name = addCategoryDto.Name,
                Description = addCategoryDto.Description,
                ParentCategoryId = addCategoryDto.ParentCategoryId
            };
            int result = await _categoryDal.AddAsync(newCategory);
            if (result > 0)
            {
                return new SuccessDataResult<CategoryDto>(_mapper.Map<CategoryDto>(newCategory), Messages.Added);
            }
            else
            {
                return new ErrorDataResult<CategoryDto>(_mapper.Map<CategoryDto>(newCategory), Messages.NotAdded);
            }
        }
        [CacheRemoveAspect(@"
        Business.Abstract.ICategoryService.GetAsync,
        Business.Abstract.ICategoryService.GetAllAsync,
        Business.Abstract.ICategoryService.GetByCategoryIdAsync,
        Business.Abstract.ICategoryService.GetChildCategoriesByCategoryIdAsync
        ")]
        public async Task<IResult> AddAsync(Category category)
        {
            int result = await _categoryDal.AddAsync(category);
            return result > 0 ? new SuccessResult(Messages.Added) : new ErrorResult(Messages.NotAdded);
        }
        [CacheRemoveAspect(@"
        Business.Abstract.ICategoryService.GetAsync,
        Business.Abstract.ICategoryService.GetAllAsync,
        Business.Abstract.ICategoryService.GetByCategoryIdAsync,
        Business.Abstract.ICategoryService.GetChildCategoriesByCategoryIdAsync
        ")]
        public async Task<IResult> DeleteAsync(int id)
        {
            bool result = false;
            Category category = await _categoryDal.GetAsync(p => p.Id == id);
            if (category != null)
            {
                result = await _categoryDal.DeleteAsync(category);
            }
            return result ? new SuccessResult("Silindi") : new ErrorResult("Silinemedi hata oluştu");
        }
        [CacheRemoveAspect(@"
        Business.Abstract.ICategoryService.GetAsync,
        Business.Abstract.ICategoryService.GetAllAsync,
        Business.Abstract.ICategoryService.GetByCategoryIdAsync,
        Business.Abstract.ICategoryService.GetChildCategoriesByCategoryIdAsync
        ")]
        public async Task<IResult> UpdateAsync(Category category)
        {
            bool result = false;
            bool isExists = await _categoryDal.IsExistAsync(p => p.Id == category.Id);
            if (!isExists)
            {
                result = await _categoryDal.UpdateAsync(category);
            }
            return result ? new SuccessResult("Güncellendi") : new ErrorResult("Güncellenemedi hata oluştu");
        }
        #endregion
        public async Task AddCategoryAsync(TrendyolCategoryDto category)
        {
            try
            {
                var result = await AddWithDtoAsync(new AddCategoryDto { Name = category.Name, Description = category.Name, ParentCategoryId = category.ParentId });
                if (result.Success)
                {
                    IEnumerable< TrendyolCategoryDto >subCategories = category.SubCategories;
                    if (subCategories != null)
                    {
                        foreach (var subCategory in subCategories)
                        {
                            subCategory.ParentId = result.Data.Id;
                            await AddCategoryAsync(subCategory);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }
    }
}