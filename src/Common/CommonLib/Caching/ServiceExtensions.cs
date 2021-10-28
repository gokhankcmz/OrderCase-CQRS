using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CommonLib.Caching
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration, string connectionString=null, string instanceName=null)
        {
            instanceName ??= configuration.GetSection("Redis:InstanceName").Value;
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = connectionString ?? configuration.GetSection("Redis:ConnectionString").Value;
                options.InstanceName = instanceName;
            });
            return services;

        }


    }
}