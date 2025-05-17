using Core.Entities;
using Core.Entities.Concrete.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Shoppings
{
    public class Favorite:BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public List<FavoriteItem> FavoriteItems { get; set; }


    }
}
