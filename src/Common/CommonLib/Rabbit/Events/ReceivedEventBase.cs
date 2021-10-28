using System;
using MediatR;

namespace CommonLib.Rabbit.Events
{
    public abstract class ReceivedEventBase
    {
        public Guid EventId { get; set; }
        public DateTime CreationTime { get; set; }

        public abstract void InformMediator(IMediator mediator);
    }
}