using System;

namespace Entities.Models
{
    public class Order : Document
    {
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = "Placed.";
        public Address Address { get; set; }
        public Product Product { get; set; }

    }
}