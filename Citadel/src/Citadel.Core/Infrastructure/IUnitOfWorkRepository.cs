using System.Data;
using System.Threading.Tasks;

namespace Citadel.Infrastructure
{
    public interface IUnitOfWorkRepository
    { 
        //新增
        Task PersistCreationOfAsync(IAggregateRoot entity, IDbConnection dbConnection, IDbTransaction dbTransaction);
        //更新
        Task PersistUpdateOfAsync(IAggregateRoot entity, IDbConnection dbConnection, IDbTransaction dbTransaction);
        //删除
        Task PersistDeletionOfAsync(IAggregateRoot entity, IDbConnection dbConnection, IDbTransaction dbTransaction);
    }
}
