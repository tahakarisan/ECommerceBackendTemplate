using Core.Entities;
using Core.Entities.Concrete.Auth;

namespace Entities.Concrete.Shoppings
{
    public class Basket : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public List<BasketItem> Items { get; set; }
    }
}
