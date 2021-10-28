using System.Reflection;
using CommonLib.Caching;
using Entities.Models;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using OrderService.Application.Behaviours;
using Repository;
using Serilog;
using Serilog.Sinks.Kafka;

namespace OrderService.Application
{
    public static class DependencyInjection
    {
        public static void AddApplication(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddAutoMapper(typeof(MappingProfile));
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            
            var mongoSettings = configuration.GetSection("MongoSettings").Get<MongoSettings>();
            services.AddSingleton(mongoSettings);
            services.AddSingleton<IMongoClient, MongoClient>(_ => new MongoClient(mongoSettings.ConnectionString));
            services.AddSingleton<IRepository<Order>, Repository<Order>>();
            services.AddRedis(configuration);
        }

        public static IHostBuilder AddSerilog(this IHostBuilder builder)
        {
            builder.UseSerilog((context, configuration) =>
            {
                configuration.Enrich.FromLogContext()
                    .Enrich.WithMachineName()
                    .WriteTo.Console()
                    .WriteTo.Kafka(context.Configuration["Kafka:bootstrapServers"],
                        topic: context.Configuration["Kafka:topic"])
                    .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                    .ReadFrom.Configuration(context.Configuration);
            });
            return builder;
        }
    }
}