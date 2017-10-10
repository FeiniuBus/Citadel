using System.Threading.Tasks;

namespace Citadel.Infrastructure
{
    public interface IMessageQueueClient
    {
        Task<IExchange> ExchangeDeclareAsync(string exchangeName, string exchangeType, bool durable, object args);
        IMessageQueueClient CreateScope();
    }
}
