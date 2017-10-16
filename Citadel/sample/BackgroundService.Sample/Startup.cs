using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Citadel;

namespace BackgroundService.Sample
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
            services.AddMvc();

            services.AddLogging();

            services.AddSingleton(new RabbitMQ.Client.ConnectionFactory()
            {
                HostName = "192.168.164.130",
                UserName = "admin",
                Password = "admin",
                Port = 5672,
                AutomaticRecoveryEnabled = true,
                NetworkRecoveryInterval = TimeSpan.FromSeconds(30)
            });

            services.AddScoped<IMessageQueueClientFactory, Citadel.Rabbit.MessageQueueClientFactory>();

            services.AddScoped<Citadel.BackgroundService.BackgroundClient>();

            services.AddScoped<Citadel.BackgroundService.ConsumerClient>();

            services.AddSingleton(new Citadel.BackgroundService.BackgroundServiceOptions
            {
                QueueName = "BackgroundService.Sample"
            });

            services.AddScoped<Citadel.BackgroundService.Bootstraper>();

            services.AddSingleton(new Citadel.Data.DbConnectionFactoryOptions()
            {
                ConnectionString = "User ID=postgres; Password=postgres; Host=192.168.164.134; Port=5432; Database=postgres;Pooling=true; Min Pool Size=0; Max Pool Size=100; Connection Lifetime=0"
            });

            services.AddScoped<Citadel.Data.IDbConnectionFactory, Citadel.Postgre.DbConnectionFactory>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddConsole(minLevel: LogLevel.Debug);

            app.UseMvc();
            using(var scope = app.ApplicationServices.CreateScope())
            {
                var bootstaper = scope.ServiceProvider.GetRequiredService<Citadel.BackgroundService.Bootstraper>();
                bootstaper.BootstrapAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            }
        }
    }
}
