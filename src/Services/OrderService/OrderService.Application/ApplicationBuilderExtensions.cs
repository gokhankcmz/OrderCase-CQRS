using CommonLib.Rabbit.Constants;
using CommonLib.Rabbit.Consumer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OrderService.Application.ReceivedEvents;

namespace OrderService.Application
{
    public static class ApplicationBuilderExtensions
    {
        public static EventConsumer<CustomerDeletedEventReceived> Listener { get; set; }

        public static IApplicationBuilder RunEventBusListener(this IApplicationBuilder app) 
        {
            Listener = app.ApplicationServices.GetService<EventConsumer<CustomerDeletedEventReceived>>();
            var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            
            life.ApplicationStarted.Register(OnStarted);
            life.ApplicationStopping.Register(OnStopping);
            return app;
        }

        private static void OnStopping()
        {
            Listener.Disconnect();
        }

        private static void OnStarted()
        {
            Listener.Consume(EventBusConstants.CustomerDeletedQueue);
        }
    }
}