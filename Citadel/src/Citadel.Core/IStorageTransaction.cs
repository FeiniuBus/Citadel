using Citadel.Data;
using System;
using System.Threading.Tasks;

namespace Citadel
{
    public interface IStorageTransaction : IDisposable
    {
        void UpdateMessage(Message message);

        void EnqueueMessage(Message message);

        Task CommitAsync();
    }
}
