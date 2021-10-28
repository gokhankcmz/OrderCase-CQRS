using System;

namespace Entities.ResponseModels
{
    public class OrderResponseDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = "Placed.";
        public string ReadFrom { get; set; }

    }
}