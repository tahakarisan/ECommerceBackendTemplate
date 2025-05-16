using Core.Entities;
using Entities.Concrete.Shoppings;

namespace Entities.DTOs.Shoppings
{
    public class AddOrderDto : IDto
    {
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
}
