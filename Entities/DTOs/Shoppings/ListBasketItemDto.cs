using Core.Entities;
using Entities.Concrete.Shoppings;

namespace Entities.DTOs.Shoppings
{
    public class ListBasketItemDto : IDto
    {
        public int BasketId { get; set; }
        public Basket Basket { get; set; }
        public int TotalPrice { get; set; }
        public int UserId { get; set; }
    }
}
