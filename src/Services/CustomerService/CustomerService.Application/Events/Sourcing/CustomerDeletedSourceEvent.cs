using System;
using CommonLib.Rabbit.Events;

namespace CustomerService.Application.Events.Sourcing
{
    public class CustomerDeletedSourceEvent : SourcingEventBase
    {
        public Guid CustomerId { get; set; }
    }
}