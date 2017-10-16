using Citadel.Data;
using Citadel.Infrastructure;
using System.Data;
using System.Threading.Tasks;

namespace Citadel
{
    public interface IMessagePersistenter
    {
        Task<string> AddMessageAsync(Message message, IDbConnection dbConnection, IDbTransaction dbTransaction);
        Task ChangeStateAsync(string messageId, MessageState messageState, IDbConnection dbConnection, IDbTransaction dbTransaction);
    }
}
