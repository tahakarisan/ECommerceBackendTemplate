using Core.Entities;

namespace Entities.Concrete.AddressConcrete
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public short PlateNo { get; set; }
        public int PhoneCode { get; set; }
        public int RowNumber { get; set; }
    }
}
