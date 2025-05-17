using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTOs.Shoppings.FavoriteModels
{
    public class AddFavoriteItemDto:IDto
    {
        public int FavoriteId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
    }
}
