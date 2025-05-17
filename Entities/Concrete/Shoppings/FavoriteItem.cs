using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete.Shoppings
{
    public class FavoriteItem:BaseEntity
    {
        public int FavoriteId { get; set; }
        public int ProductId { get; set; }
        public Favorite Favorite { get; set; }
        public Product Product { get; set; }
        public int UserId { get; set; }
    }
}
