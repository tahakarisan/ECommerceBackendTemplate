using Core.DataAccess;
using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete.Shoppings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.Shoppings
{
    public class EfFavoriteItemDal:EfEntityRepositoryBase<FavoriteItem,ECommerceContext>,IFavoriteItemDal
    {
    }
}
