using Microsoft.Extensions.DependencyInjection;
using System;

namespace Citadel.BackgroundService.Client
{
    public class BackgroundServiceModule : IModule
    {
        public void StartUp(IServiceProvider serviceProvider)
        {
            var consumerClient = serviceProvider.GetRequiredService<ConsumerClient>();
            consumerClient.InitClientAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            consumerClient.ConsumeAsync().ConfigureAwait(false).GetAwaiter().GetResult();
        }
    }
}
