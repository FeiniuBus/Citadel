using System.Collections.Generic;

namespace Citadel.Infrastructure
{
    public interface IDeliverEventArgs
    {
        IReadOnlyDictionary<string, object> Properties { get; }
        TransferMessage Content { get; }

        void BasicAck();
        void BasicNack(bool requeue);
    }
}
