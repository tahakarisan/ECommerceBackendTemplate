using DataAccess.Abstract;
using DataAccess.Abstract.AddressAbstract;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.EntityFramework.AddressEfConcrete;
using DataAccess.Concrete.EntityFramework.Auths;
using DataAccess.Concrete.EntityFramework.Shoppings;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess
{
    public static class ServiceRegistration
    {
        public static void AddDataAccessRegistration(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IProductDal, EfProductDal>();
            serviceCollection.AddSingleton<ICategoryDal, EfCategoryDal>();
            serviceCollection.AddSingleton<IUserDal, EfUserDal>();
            serviceCollection.AddSingleton<IOperationClaimDal, EfOperationClaimDal>();
            serviceCollection.AddSingleton<IUserOperationClaimDal, EfUserOperationClaimDal>();
            serviceCollection.AddSingleton<IResetPasswordCodeDal, EfResetPasswordCodeDal>();
            serviceCollection.AddSingleton<IProductImageDal, EfProductImageDal>();
            #region Address
            serviceCollection.AddSingleton<IAddressDal, EfAddressDal>();
            serviceCollection.AddSingleton<ICountryDal, EfCountryDal>();
            serviceCollection.AddSingleton<ICityDal, EfCityDal>();
            serviceCollection.AddSingleton<ICountyDal, EfCountyDal>();
            serviceCollection.AddSingleton<IDistrictDal, EfDistrictDal>();
            #endregion
            #region Shopping
            serviceCollection.AddSingleton<IBasketDal, EfBasketDal>();
            serviceCollection.AddSingleton<IOrderDal, EfOrderDal>();
            serviceCollection.AddSingleton<IBasketItemDal, EfBasketItemDal>();
            serviceCollection.AddSingleton<IOrderItemDal, EfOrderItemDal>();
            #endregion
            serviceCollection.AddSingleton<IBrandDal, EfBrandDal>();
        }
    }
}
