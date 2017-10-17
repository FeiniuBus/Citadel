using System;

namespace Citadel
{
    public interface IModule
    {
        void StartUp(IServiceProvider serviceProvider);
    }
}
