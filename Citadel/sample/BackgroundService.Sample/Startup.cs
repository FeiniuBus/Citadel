using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

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

            services.AddCitadel(options =>
            {
                options.UseRabbitMQ(rabbit =>
                {
                    rabbit.HostName = "192.168.164.130";
                    rabbit.UserName = "admin";
                    rabbit.Password = "admin";
                    rabbit.Port = 5672;
                    rabbit.AutomaticRecoveryEnabled = true;
                    rabbit.NetworkRecoveryInterval = TimeSpan.FromSeconds(15);
                });

                options.UsePostgreSQL(postgre =>
                {
                    postgre.ConnectionString = "Host=192.168.164.134; Database=postgres;User ID=postgres; Password=kge2001; Port=5432; Pooling=true; ";
                    postgre.Schema = "public";
                });

                options.UseBackgroundService(backgroundService =>
                {
                    backgroundService.Exchange = "backgroundService.Sample";
                });
            });
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

            app.UseCitadel();
        }
    }
}
