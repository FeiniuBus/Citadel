using Microsoft.Extensions.DependencyInjection;

namespace Citadel.Configuration
{
    public interface ICitadelExtension
    {
        void AddServices(IServiceCollection services);
    }
}
