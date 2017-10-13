using System.Collections.Generic;

namespace Citadel.Infrastructure
{
    public interface IDeliverEventArgs
    {
        IReadOnlyDictionary<string, object> Properties { get; }
        byte[] Content { get; }

        void BasicAck();
        void BasicNack(bool requeue);
    }
}
