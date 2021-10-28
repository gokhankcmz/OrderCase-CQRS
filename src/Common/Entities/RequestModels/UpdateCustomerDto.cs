using Entities.Models;

namespace Entities.RequestModels
{
    public class UpdateCustomerDto
    {
        public string Name { get; set; }
        
        public Address Address { get; set; }
        public string Email { get; set; }
    }
}