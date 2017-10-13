using System.Threading.Tasks;

namespace Citadel
{
    public interface IMessageQueueClient
    {
        Task<IExchange> ExchangeDeclareAsync(string exchangeName, string exchangeType, bool durable, object args);
        Task<IExchange> DelayExchangeDeclareAsync(string exchangeName,string exchangeType, bool durable, object args);
        IMessageQueueClient CreateScope();
    }
}
