using System.Threading.Tasks;

namespace Citadel.Infrastructure
{
    public interface IExchange
    {
        Task<IQueue> QueueDeclareAsync(string queueName, string queueType, object args);

    }
}
