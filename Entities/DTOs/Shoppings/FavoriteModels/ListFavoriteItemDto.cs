using Core.Entities;
using Entities.Concrete.Shoppings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Shoppings.FavoriteModels
{
    public class ListFavoriteItemDto:IDto
    {
        public int FavoriteId { get; set; }
        public Favorite Favorite{ get; set; }
        public int UserId { get; set; }

    }
}
