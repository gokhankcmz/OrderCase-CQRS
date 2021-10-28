using System;
using CommonLib.Rabbit.Events;
using MediatR;
using OrderService.Application.Commands;

namespace OrderService.Application.ReceivedEvents
{
    public class CustomerDeletedEventReceived : ReceivedEventBase
    {
        public Guid CustomerId { get; set; }

        public override void InformMediator(IMediator mediator)
        {
            var command = new DeleteOrdersByCustomerIdCommand {CustomerId = CustomerId};
            mediator.Send(command);
        }
    }
}