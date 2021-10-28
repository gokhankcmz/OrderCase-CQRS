namespace Entities.Models
{
    public class Address
    {
        public string AddressLine { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int CityCode { get; set; }
        
        public object ShallowCopy()
        {
            return MemberwiseClone();
        }
    }
}