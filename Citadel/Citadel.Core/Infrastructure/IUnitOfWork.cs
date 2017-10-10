using System.Data;
using System.Threading.Tasks;

namespace Citadel.Infrastructure
{
    public interface IUnitOfWork
    {
        void RegisterAdd(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository);

        void RegisterUpdate(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository);

        void RegisterRemoved(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository);

        Task CommitAsync(IDbConnection dbConnection);

        Task CommitAsync(IDbConnection dbConnection, IDbTransaction dbTransaction);
    }
}
