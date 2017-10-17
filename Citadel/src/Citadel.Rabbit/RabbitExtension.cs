using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System;

namespace Citadel.Rabbit
{
    public class RabbitExtension : Configuration.ICitadelExtension
    {
        private readonly Action<ConnectionFactory> _configure;

        public RabbitExtension(Action<ConnectionFactory> configure)
        {
            _configure = configure;
        }


        public void AddServices(IServiceCollection services)
        {
            var connectionFactory = new ConnectionFactory();
            _configure(connectionFactory);

            services.AddSingleton(connectionFactory);
            services.AddScoped<IMessageQueueClientFactory, MessageQueueClientFactory>();
        }
    }
}
