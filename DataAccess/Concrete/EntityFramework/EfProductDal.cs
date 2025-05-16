using Core.DataAccess.EntityFramework;
using Core.Utilities.Paging;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.DTOs.Products;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfProductDal : EfEntityRepositoryBase<Product, ECommerceContext>, IProductDal
    {
        #region Queries
        public async Task<Product> GetMostExpensiveProductAsync()
        {
            using (ECommerceContext context = new ECommerceContext())
            {
                return await context.Products.OrderByDescending(m => m.Price).FirstOrDefaultAsync();
            }
        }
        public async Task<IPaginate<ProductDetailDto>> GetProductDetailDtoAsync(int index, int size)
        {
            return await genericProductDetailDtoAsync(filter: null, index: index, size: size);
        }
        public async Task<IPaginate<ProductDetailDto>> GetProductDetailDtoByCategoryIdAsync(int categoryId, int index, int size)
        {
            return await genericProductDetailDtoAsync(filter: p => p.CategoryId == categoryId, index: index, size: size);
        }
        public async Task<IPaginate<ProductDetailDto>> genericProductDetailDtoAsync(Expression<Func<Product, bool>> filter = null, int index = 0, int size = 10, bool doPaginate = true)
        {
            using (ECommerceContext context = new ECommerceContext())
            {
                IQueryable<ProductDetailDto> result = from p in filter == null ? context.Products : context.Products.Where(filter)
                                                      join c in context.Categories on p.CategoryId equals c.Id
                                                      join brand in context.Brands on p.BrandId equals brand.Id
                                                      let images = (from images in context.ProductImages where p.Id == images.ProductId select images.ImageUrl).ToList()
                                                      select new ProductDetailDto
                                                      {
                                                          //Product
                                                          Id = p.Id,
                                                          Name = p.Name,
                                                          Price = p.Price,
                                                          StockQuantity = p.StockQuantity,
                                                          CreatedDate = p.CreatedDate,
                                                          UpdatedDate = p.UpdatedDate,
                                                          //Category
                                                          CategoryId = c.Id,
                                                          CategoryName = c.Name,
                                                          //Images
                                                          ProductImages = images,
                                                          //Brand
                                                          BrandId = brand.Id,
                                                          BrandName = brand.Name,
                                                      };
                if (doPaginate)
                    return await result.ToPaginateAsync(index, size);
                else
                    return new Paginate<ProductDetailDto> { Items = new List<ProductDetailDto>(result) };
            }
        }
        public async Task<ProductDetailDto> GetProductDetailByIdAsync(int id)
        {
            IPaginate<ProductDetailDto> result = await genericProductDetailDtoAsync(filter: p => p.Id == id, doPaginate: false);
            return result.Items.Count != 0 ? result.Items.FirstOrDefault() : null;
        }
        public async Task<int> GetProductsCountFromDalAsync()
        {
            using (ECommerceContext context = new ECommerceContext())
            {
                return await context.Products.CountAsync();
            }
        }
        public async Task<List<ProductDetailDto>> GetPopularProductsAsync(int index = 0, int size = 20)
        {
            using (ECommerceContext context = new ECommerceContext())
            {
                var result = await (
                     from bi in context.BasketItems
                     join p in context.Products on bi.ProductId equals p.Id
                     group bi by new { p.Id, p.Name } into g
                     orderby g.Sum(bi => bi.Quantity) descending
                     select new
                     {
                         Id = g.Key.Id,
                         TotalSales = g.Sum(bi => bi.Quantity)
                     }
                    ).Take(20).ToListAsync();

                IEnumerable<ProductDetailDto> popularProducts = (
                    from r in result
                    join p in context.Products on r.Id equals p.Id
                    join c in context.Categories on p.CategoryId equals c.Id
                    join brand in context.Brands on p.BrandId equals brand.Id
                    let images = (
                        from images in context.ProductImages
                        where p.Id == images.ProductId
                        select images.ImageUrl
                    ).ToList()
                    select new ProductDetailDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        CategoryId = c.Id,
                        CategoryName = c.Name,
                        ProductImages = images,
                        BrandId = brand.Id,
                        BrandName = brand.Name
                    }
                );
                return popularProducts.ToList();
            }
        }
        #endregion
    }
}