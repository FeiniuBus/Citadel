using Microsoft.Extensions.DependencyInjection;
using System;

namespace Citadel
{
    public class CitadelBuilder
    {

        public CitadelBuilder(IServiceCollection serviceCollection)
        {
            Services = serviceCollection;
        }

        /// <summary>
        /// Gets the <see cref="IServiceCollection" /> where MVC services are configured.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Adds a scoped service of the type specified in serviceType with an implementation
        /// </summary>
        private CitadelBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }
    }
}
