using Citadel;
using Citadel.Server;
using Microsoft.AspNetCore.Builder;
using System.Linq;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AppBuilderExtensions
    {
        public static void UseCitadel(this IApplicationBuilder builder)
        {
            var scope = builder.ApplicationServices.CreateScope();
            var storageModule = scope.ServiceProvider.GetRequiredService<StorageModule>();
            storageModule.StartUp(scope.ServiceProvider);

            var modules = scope.ServiceProvider.GetServices<IModule>();
            if(modules?.Any() == true)
            {
                foreach(var module in modules)
                {
                    module.StartUp(builder.ApplicationServices.CreateScope().ServiceProvider);
                }
            }
        }
    }
}
