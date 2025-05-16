using Core.Entities;

namespace Entities.Concrete.AddressConcrete
{
    public class Country : BaseEntity
    {
        public string CountryName { get; set; }
        public string BinaryCode { get; set; }
        public string TernaryCode { get; set; }
        public string PhoneCode { get; set; }
    }
}
