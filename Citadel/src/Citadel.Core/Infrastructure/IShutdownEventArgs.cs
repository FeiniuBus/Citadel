using System.Collections.Generic;

namespace Citadel.Infrastructure
{
    public interface IShutdownEventArgs
    {
        object Cause { get; }

        IReadOnlyDictionary<string, object> Infomations { get; }
    }
}
