using System;
using Entities.Models;

namespace Entities.ResponseModels
{
    public class OrderCollectionDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; } = "Placed.";
        public string Address { get; set; }
        public Product Product { get; set; }
    }
}