using Citadel.Data;
using Citadel.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Citadel.Postgre
{
    public class PostgreExtension : Configuration.ICitadelExtension
    {
        private readonly Action<DbConnectionFactoryOptions> _configure;

        public PostgreExtension(Action<DbConnectionFactoryOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            var options = new DbConnectionFactoryOptions();
            _configure(options);
            services.AddSingleton(options);
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddScoped<IStorage, Storage>();
            services.AddScoped<IMessagePersistenter, MessagePersistenter>();
        }
    }
}
