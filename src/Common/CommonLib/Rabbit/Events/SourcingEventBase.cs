using System;

namespace CommonLib.Rabbit.Events
{
    public abstract class SourcingEventBase
    {
        public Guid EventId { get; }
        public DateTime CreationTime { get;}

        
        public SourcingEventBase()
        {
            CreationTime = DateTime.UtcNow;
            EventId = Guid.NewGuid();
        }

    }
}