using System;
using RabbitMQ.Client;

namespace CommonLib.Rabbit.Utility
{
    public interface IRabbitMqPersistentConnection : IDisposable
    {
        bool IsConnected { get; }
        bool TryConnect();
        IModel CreateModel();

        int GetRetryCount();
    }
}