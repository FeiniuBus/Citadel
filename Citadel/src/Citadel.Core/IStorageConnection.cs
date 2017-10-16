using Citadel.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Citadel
{
    public interface IStorageConnection
    {
        Task<Message> GetMessageAsync(string id);

        Task<IFetchedMessage> FetchNextMessageAsync();

        Task<Message> GetNextMessageToBeEnqueuedAsync();

        Task<IEnumerable<Message>> GetFailedPublishedMessagesAsync();
    }
}
