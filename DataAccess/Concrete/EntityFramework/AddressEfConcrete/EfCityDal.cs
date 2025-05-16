using Core.DataAccess.EntityFramework;
using DataAccess.Abstract.AddressAbstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.AddressConcrete;

namespace DataAccess.Concrete.EntityFramework.AddressEfConcrete
{
    public class EfCityDal : EfEntityRepositoryBase<City, ECommerceContext>, ICityDal
    {
    }
}
