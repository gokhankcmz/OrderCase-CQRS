using System;
using System.Text;
using CommonLib.Rabbit.Events;
using CommonLib.Rabbit.Utility;
using MediatR;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CommonLib.Rabbit.Consumer
{
    public class EventConsumer<T> where T : ReceivedEventBase
    {
        private readonly IRabbitMqPersistentConnection _persistentConnection;
        private readonly IMediator _mediator;

        public EventConsumer(IRabbitMqPersistentConnection persistentConnection, IMediator mediator)
        {
            _persistentConnection = persistentConnection;
            _mediator = mediator;
        }

        public void Consume(string queue)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();
            channel.QueueDeclare(queue, true, false, false, null);
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += RecievedEvent;

            //channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            channel.BasicConsume(queue, autoAck:true, consumer: consumer);
        }

        private void RecievedEvent(object sender, BasicDeliverEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Body.Span);
            Console.WriteLine("Message: " + message);
            var incomingEvent = JsonConvert.DeserializeObject<T>(message);
            incomingEvent?.InformMediator(_mediator);
        }

        public void Disconnect()
        {
            _persistentConnection.Dispose();
        }
    }
}