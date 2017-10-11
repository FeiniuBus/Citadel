using System.Threading.Tasks;

namespace Citadel.Infrastructure
{
    public interface IExchange
    {
        string ExchangeName { get;  }
        string ExchangeType { get;  }
        T GetArgumentValue<T>(string key);

        Task<QueueDeclareResult> QueueDeclareAsync(string queueName, QueueDeclareOptions options, object args);
        Task PublishAsync<TContent>(string topic, TContent content, object arguments, ISerializer serializer = null);
    }
}
