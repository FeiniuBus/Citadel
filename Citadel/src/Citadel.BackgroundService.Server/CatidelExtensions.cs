using Citadel;
using Citadel.BackgroundService;
using Citadel.BackgroundService.Server;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CatidelExtensions
    {
        public static void UseBackgroundService(this CitadelOptions options, Action<BackgroundServiceOptions> configure)
        {
            var extension = new BackgroundServiceExtension(configure);
            options.RegisterExtension(extension);
        }
    }
}
