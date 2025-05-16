using Core.Entities;

namespace Entities.Concrete.AddressConcrete
{
    public class County : BaseEntity
    {
        public string Name { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
    }
}
