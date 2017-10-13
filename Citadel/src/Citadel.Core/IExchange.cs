using Citadel.Infrastructure;
using System.Threading.Tasks;

namespace Citadel
{
    public interface IExchange
    {
        string ExchangeName { get;  }
        string ExchangeType { get;  }
        T GetArgumentValue<T>(string key);

        Task<QueueDeclareResult> QueueDeclareAsync(string queueName, QueueDeclareOptions options, object args);
        Task PublishAsync(string topic, byte[] content, object arguments);
    }
}
