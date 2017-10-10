namespace Citadel.Infrastructure
{
    public interface IUnitOfWorkRepository
    { 
        //新增
        void PersistCreationOf(IAggregateRoot entity);
        //更新
        void PersistUpdateOf(IAggregateRoot entity);
        //删除
        void PersistDeletionOf(IAggregateRoot entity);
    }
}
