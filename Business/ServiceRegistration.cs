using Business.Abstract;
using Business.Abstract.Addresses;
using Business.Abstract.Auths;
using Business.Abstract.Shoppings;
using Business.Concrete;
using Business.Concrete.Addresses;
using Business.Concrete.Auths;
using Business.Concrete.Shoppings;
using Business.Utilities.File;
using Business.Utilities.Mail;
using Core.Utilities.Security.JWT;
using Microsoft.Extensions.DependencyInjection;

namespace Business
{
    public static class ServiceRegistration
    {
        public static void AddBusinessRegistration(this IServiceCollection serviceCollection)
        {
            #region Persistence
            serviceCollection.AddSingleton<IProductService, ProductManager>();
            serviceCollection.AddSingleton<ICategoryService, CategoryManager>();
            serviceCollection.AddSingleton<IAuthService, AuthManager>();
            serviceCollection.AddSingleton<IResetPasswordCodeService, ResetPasswordCodeManager>();
            serviceCollection.AddSingleton<IUserService, UserManager>();
            serviceCollection.AddSingleton<IAuthService, AuthManager>();
            serviceCollection.AddSingleton<IProductImageService, ProductImageManager>();
            #region Address
            serviceCollection.AddSingleton<IAddressService, AddressManager>();
            serviceCollection.AddSingleton<ICountryService, CountryManager>();
            serviceCollection.AddSingleton<ICityService, CityManager>();
            serviceCollection.AddSingleton<ICountyService, CountyManager>();
            serviceCollection.AddSingleton<IDistrictService, DistrictManager>();
            #endregion
            #region Shopping
            serviceCollection.AddSingleton<IBasketService, BasketManager>();
            serviceCollection.AddSingleton<IOrderService, OrderManager>();
            serviceCollection.AddSingleton<IBasketItemService, BasketItemManager>();
            serviceCollection.AddSingleton<IOrderItemService, OrderItemManager>();
            #endregion
            #endregion
            #region Infastructure
            serviceCollection.AddSingleton<IMailService, MailManager>();
            serviceCollection.AddSingleton<IProductImageUploadService, ProductImageUploadManager>();
            serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            serviceCollection.AddSingleton<ITokenHelper, JwtHelper>();
            #endregion
        }
    }
}
