using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using CommonLib.Rabbit.Events;
using CommonLib.Rabbit.Utility;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client.Exceptions;

namespace CommonLib.Rabbit.Producer
{
    public class EventProducer
    {
        
        private readonly ILogger<EventProducer> _logger;
        private readonly IRabbitMqPersistentConnection _persistentConnection;
        private readonly int _retryCount;

        public EventProducer(ILogger<EventProducer> logger, IRabbitMqPersistentConnection persistentConnection)
        {
            _logger = logger;
            _persistentConnection = persistentConnection;
            _retryCount = _persistentConnection.GetRetryCount();
        }

        public void Publish<T>(T eventToPublish, QueueOptions queueOptions, ExchangeOptions exchangeOptions,
            string messageRoutingKey=null) where T: SourcingEventBase
        {

            messageRoutingKey = string.IsNullOrEmpty(messageRoutingKey) ? queueOptions.RoutingKey : messageRoutingKey;
            
            if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();
            var policy = Policy.Handle<SocketException>()
                .Or<BrokerUnreachableException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (ex, time) =>
                    {
                        _logger.LogWarning(ex,
                            "RabbitMQ Client could not connect after {TimeOut}s ({ExceptionMessage})",
                            $"{time.TotalSeconds:n1}", ex);
                    });
            
            
            using (var channel = _persistentConnection.CreateModel())
            {
                channel.ExchangeDeclare(
                    exchange: exchangeOptions.ExchangeName,
                    type: exchangeOptions.ExchangeType,
                    durable: exchangeOptions.Durable, 
                    autoDelete: exchangeOptions.AutoDelete,
                    arguments:exchangeOptions.Arguments);

                channel.QueueDeclare(
                    queue: queueOptions.QueueName,
                    durable: queueOptions.Durable,
                    exclusive: queueOptions.Exclusive,
                    autoDelete: queueOptions.AutoDelete,
                    arguments: queueOptions.Arguments);
                
                
                channel.QueueBind(
                    queue:queueOptions.QueueName,
                    exchange:exchangeOptions.ExchangeName,
                    arguments: null,
                    routingKey:queueOptions.RoutingKey);

                var message = JsonConvert.SerializeObject(eventToPublish);
                var body = Encoding.UTF8.GetBytes(message);
                
                policy.Execute(() =>
                {
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    properties.DeliveryMode = 2;
                    channel.ConfirmSelect();
                    channel.BasicPublish(
                        
                        exchange: exchangeOptions.ExchangeName,
                        routingKey:messageRoutingKey, 
                        mandatory:true,
                        basicProperties:properties,
                        body);
                    channel.WaitForConfirmsOrDie();
                    channel.BasicAcks += (sender, eventArgs) =>
                    {
                        Console.WriteLine("Sent. RabbitMQ");
                        //Ack implementation.
                    };
                });
            }
        }
    }
}