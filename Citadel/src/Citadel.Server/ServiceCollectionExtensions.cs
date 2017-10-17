using Citadel;
using Citadel.Server;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static CitadelBuilder AddCitadel(this IServiceCollection services, Action<CitadelServerOptions> configure)
        {
            services.AddScoped<StorageModule>();


            var options = new CitadelServerOptions();
            configure(options);

            foreach (var extension in options.Extensions)
            {
                extension.AddServices(services);
            }
            services.AddSingleton(options);

            return new CitadelBuilder(services);
        }
    }
}
