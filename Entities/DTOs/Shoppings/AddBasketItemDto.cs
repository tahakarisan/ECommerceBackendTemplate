using Core.Entities;

namespace Entities.DTOs.Shoppings
{
    public class AddBasketItemDto : IDto
    {
        public int BasketId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int UserId { get; set; }
    }
}
