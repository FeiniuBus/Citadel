using System;
using System.Data;
using System.Threading.Tasks;

namespace Citadel.Data
{
    public interface IFetchedMessage : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        string MessageId { get; set; }
        string MessageType { get; set; }

        Task CommitAsync();

        Task RollbackAsync();
    }
}
