using CommonLib.Jwt;
using CommonLib.Rabbit.Utility;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using OrderService.Application;
using OrderService.Application.ReceivedEvents;
using OrderService.Command.Api.Middlewares;

namespace OrderService.Command.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplication(Configuration);
            services.AddControllers().AddFluentValidation(x =>
            {
                x.AutomaticValidationEnabled = false;
                x.ImplicitlyValidateChildProperties = true;
            });
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "OrderService.Command.Api", Version = "v1"});
            });
            services.ConfigureJwt(Configuration);
            var rabbitSettings = Configuration.GetSection("RabbitMqSettings").Get<RabbitMqSettings>();;
            services.AddRabbitMq(rabbitSettings).RegisterAsConsumer<CustomerDeletedEventReceived>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.RunEventBusListener();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService.Command.Api v1"));
            app.UseHttpsRedirection();
            app.UseCustomErrorHandler();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}