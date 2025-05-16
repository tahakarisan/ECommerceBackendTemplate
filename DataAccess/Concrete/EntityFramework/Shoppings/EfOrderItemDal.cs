using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.Shoppings;

namespace DataAccess.Concrete.EntityFramework.Shoppings
{
    public class EfOrderItemDal : EfEntityRepositoryBase<OrderItem, ECommerceContext>, IOrderItemDal
    {
    }
}
