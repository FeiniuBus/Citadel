using System.Data;
using System.Threading.Tasks;
using Citadel.Infrastructure;

namespace Citadel.Abstracts
{
    public abstract class AbstractRepository<TEntity> : IUnitOfWorkRepository
        where TEntity: class, IAggregateRoot, new()
    {
        protected readonly IUnitOfWork UnitOfWork;

        public AbstractRepository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public abstract Task PersistCreationOfAsync(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction);

        public abstract Task PersistDeletionOfAsync(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction);

        public abstract Task PersistUpdateOfAsync(TEntity entity, IDbConnection dbConnection, IDbTransaction dbTransaction);

        Task IUnitOfWorkRepository.PersistCreationOfAsync(IAggregateRoot entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            if (entity is TEntity == false) throw new System.Exception($"Expecting {typeof(TEntity).Name} but {entity.GetType().Name}");
            return PersistCreationOfAsync((TEntity)entity, dbConnection, dbTransaction);
        }

        Task IUnitOfWorkRepository.PersistDeletionOfAsync(IAggregateRoot entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            if (entity is TEntity == false) throw new System.Exception($"Expecting {typeof(TEntity).Name} but {entity.GetType().Name}");
            return PersistDeletionOfAsync((TEntity)entity, dbConnection, dbTransaction);
        }

        Task IUnitOfWorkRepository.PersistUpdateOfAsync(IAggregateRoot entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            if (entity is TEntity == false) throw new System.Exception($"Expecting {typeof(TEntity).Name} but {entity.GetType().Name}");
            return PersistUpdateOfAsync((TEntity)entity, dbConnection, dbTransaction);
        }
    }
}
