using Core.Entities;

namespace Entities.Concrete.AddressConcrete
{
    public class District : BaseEntity
    {
        public string Name { get; set; }
        public string NeighbourhoodName { get; set; }
        public string PostCode { get; set; }
        public string CountyId { get; set; }
    }
}
