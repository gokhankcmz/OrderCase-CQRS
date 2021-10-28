using CommonLib.Rabbit.Consumer;
using CommonLib.Rabbit.Events;
using CommonLib.Rabbit.Producer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace CommonLib.Rabbit.Utility
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, RabbitMqSettings rabbitSettings)
        {
            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();
                var factory = new ConnectionFactory
                {
                    HostName = rabbitSettings.Hostname,
                    Password = rabbitSettings.Password,
                    UserName = rabbitSettings.Username,
                    Port = rabbitSettings.Port
                };
                return new DefaultRabbitMqPersistentConnection(factory, rabbitSettings.RetryCount, logger);
            });
            return services;
        }

        public static IServiceCollection RegisterAsProducer(this IServiceCollection services) => services.AddSingleton<EventProducer>();
        public static IServiceCollection RegisterAsConsumer<T>(this IServiceCollection services) where T : ReceivedEventBase => services.AddSingleton<EventConsumer<T>>();
    }
}