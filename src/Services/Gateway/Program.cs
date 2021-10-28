using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Gateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // .UseSerilog((context, configuration) =>
                // {
                //     configuration.Enrich.FromLogContext()
                //         .Enrich.WithMachineName()
                //         .WriteTo.Console()
                //         .WriteTo.Kafka(context.Configuration["Kafka:bootstrapServers"], topic:context.Configuration["Kafka:topic"])
                //         .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                //         .ReadFrom.Configuration(context.Configuration);
                // })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
    }
}