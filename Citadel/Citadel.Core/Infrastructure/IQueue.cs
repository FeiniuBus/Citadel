using System;
using System.Threading.Tasks;

namespace Citadel.Infrastructure
{
    public interface IQueue
    {
        Task<MessageContext> ConsumeAsync();

        void Subscribe(Action<MessageContext> callback);

        Task BindToExchangeAsync(string exchangeName);
    }
}
