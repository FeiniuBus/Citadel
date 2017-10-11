using System;
using System.Threading.Tasks;

namespace Citadel.Infrastructure
{
    public interface IDelayExchange : IExchange
    {
        Task DelayAsync<TContent>(string topic, TimeSpan time, TContent content, object arguments, ISerializer serializer = null);
    }
}
