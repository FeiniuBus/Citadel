using System;
using System.Data;
using System.Threading.Tasks;

namespace Citadel.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        void RegisterAdd(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository);

        void RegisterUpdate(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository);

        void RegisterRemoved(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository);

        Task CommitAsync();

        IDbConnection GetDbConnection();
        IDbTransaction GetCurrentDbTransaction();

        IsolationLevel IsolationLevel { get; set; }
    }
}
