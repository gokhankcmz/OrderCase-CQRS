using System;

namespace Entities.ResponseModels
{
    public class CustomerCollectionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}