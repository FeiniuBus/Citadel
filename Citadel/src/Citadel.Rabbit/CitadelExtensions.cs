using Citadel;
using Citadel.Rabbit;
using RabbitMQ.Client;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CitadelExtensions 
    {
        public static void UseRabbitMQ(this CitadelOptions options, Action<ConnectionFactory> configure)
        {
            var extension = new RabbitExtension(configure);
            options.RegisterExtension(extension);
        }
    }
}
