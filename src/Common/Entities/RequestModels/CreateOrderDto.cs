using System;
using Entities.Models;

namespace Entities.RequestModels
{
    public class CreateOrderDto
    {
        public Guid CustomerId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public Product Product { get; set; }

    }
}