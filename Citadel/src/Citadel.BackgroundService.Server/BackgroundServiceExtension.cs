using Citadel.Configuration;
using Citadel.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Citadel.BackgroundService.Server
{
    public class BackgroundServiceExtension : ICitadelExtension
    {
        private readonly Action<BackgroundServiceOptions> _configure;

        public BackgroundServiceExtension(Action<BackgroundServiceOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            var options = new BackgroundServiceOptions();
            _configure(options);

            services.AddSingleton(options);
            services.AddScoped<IStorage, Storage>();
            services.AddScoped<BackgroundClient>();
            services.AddScoped<JobPersistenter>();
        }
    }
}
