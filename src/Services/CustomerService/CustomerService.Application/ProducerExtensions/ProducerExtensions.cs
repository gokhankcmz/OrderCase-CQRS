using System;
using CommonLib.Rabbit.Constants;
using CommonLib.Rabbit.Producer;
using CommonLib.Rabbit.Utility;
using CustomerService.Application.Events.Sourcing;
using RabbitMQ.Client;

namespace CustomerService.Application.ProducerExtensions
{
    public static class ProducerExtensions
    {
        public static void PublishCustomerDeleted(this EventProducer producer, Guid customerId)
        {
            
            var queueOptions = new QueueOptions
            {
                Arguments = null,
                Durable = true,
                Exclusive = false,
                AutoDelete = false,
                QueueName = EventBusConstants.CustomerDeletedQueue,
                RoutingKey = EventBusConstants.CustomerDeletedQueue
            };

            var exchangeOptions = new ExchangeOptions
            {
                Arguments = null,
                Durable = true,
                AutoDelete = false,
                ExchangeName = "CustomerDeleted",
                ExchangeType = ExchangeType.Topic
            };

            var customerDeletedEvent = new CustomerDeletedSourceEvent {CustomerId = customerId};
            producer.Publish<CustomerDeletedSourceEvent>(customerDeletedEvent, queueOptions, exchangeOptions);
            
        }

        
    }
}