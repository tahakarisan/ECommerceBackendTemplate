using AutoMapper;
using Entities.Concrete;
using Entities.Concrete.AddressConcrete;
using Entities.Concrete.Shoppings;
using Entities.DTOs.Addresses;
using Entities.DTOs.Categories;
using Entities.DTOs.Categories.NotRelatedModels;
using Entities.DTOs.Products;
using Entities.DTOs.Products.NotRelatedModels;
using Entities.DTOs.Shoppings;
using Entities.DTOs.Shoppings.NotRelatedModels;

namespace Business.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            #region Category Mapping
            CreateMap<AddCategoryDto, Category>().ReverseMap();
            CreateMap<AddChildCategoryDto, Category>().ReverseMap();
            CreateMap<CategoryNotRelated, Category>().ReverseMap();
            CreateMap<CategoryDto, Category>().ReverseMap();
            CreateMap<CategoryDto, CategoryNotRelated>().ReverseMap();
            #endregion
            #region Products Mapping
            CreateMap<AddProductDto, Product>().ReverseMap();
            CreateMap<AddProductWithImageDto, Product>().ReverseMap();
            CreateMap<ProductNotRelated, Product>().ReverseMap();
            CreateMap<ProductDto, Product>().ReverseMap();
            CreateMap<ProductDto, ProductNotRelated>().ReverseMap();
            CreateMap<BrandNotRelated, Brand>().ReverseMap();
            CreateMap<BrandDto, Brand>().ReverseMap();
            CreateMap<BrandDto, BrandNotRelated>().ReverseMap();
            #endregion
            #region Shopping
            CreateMap<AddBasketDto, Basket>().ReverseMap();
            CreateMap<AddBasketItemDto, BasketItem>().ReverseMap();
            CreateMap<AddOrderDto, Order>().ReverseMap();
            CreateMap<AddOrderItemDto, OrderItem>().ReverseMap();
            CreateMap<BasketItemNotRelated, BasketItem>().ReverseMap();
            #endregion

            CreateMap<AddAddressDto, Address>().ReverseMap();

        }
    }
}
