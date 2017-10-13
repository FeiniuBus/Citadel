using Citadel.Infrastructure;
using System.Threading.Tasks;

namespace Citadel
{
    public interface IConsumer
    {
        IQueue Queue { get; }

        string Topic { get;  }
        T GetArgumentValue<T>(string key);

        event DeliverEventHandler OnDelivered;
        event ShutdownEventHandler OnShutdown;

        Task SubscribeAsync();
    }
}
