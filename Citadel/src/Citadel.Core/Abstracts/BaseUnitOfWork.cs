using Citadel.Data;
using Citadel.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Citadel.Abstracts
{
    public abstract class BaseUnitOfWork : IUnitOfWork
    {
        private readonly WorkUnitOptions _dbOptions;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private IDbConnection _dbConnection;
        private IDbTransaction _dbTransaction;
        private readonly Queue<WorkUnitDescriptor> _descriptors;

        public BaseUnitOfWork(WorkUnitOptions options, IDbConnectionFactory dbConnectionFactory) : this(options, dbConnectionFactory, global::System.Data.IsolationLevel.ReadCommitted) { }

        public BaseUnitOfWork(WorkUnitOptions options, IDbConnectionFactory dbConnectionFactory, IsolationLevel isolationLevel)
        {
            _dbOptions = options;
            _dbConnectionFactory = dbConnectionFactory;
            _descriptors = new Queue<WorkUnitDescriptor>();
            IsolationLevel = IsolationLevel;
        }

        public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.ReadCommitted;

        public virtual async Task CommitAsync()
        {
            if (_descriptors.Count == 0) return;

            var dbConnection = GetDbConnection();
            IDbTransaction dbTransaction = null;
            dbConnection.Open();
            dbTransaction = GetOrStartTransaction();

            while (_descriptors.TryDequeue(out WorkUnitDescriptor descriptor))
            {
                await PersistAsync(descriptor, dbConnection, dbTransaction);
            }


           dbTransaction.Commit();
        }

        public IDbConnection GetDbConnection()
        {
            if(_dbConnection == null)
            {
                _dbConnection = _dbConnectionFactory.CreateDbConnection(_dbOptions.ConnectionString);
            }

            return _dbConnection;
        }

        public IDbTransaction GetCurrentDbTransaction() => _dbTransaction;

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            if (_dbConnection?.State != ConnectionState.Open) throw new Exception("Connection must be opened before starting transaction.");
            if (_dbTransaction != null) throw new Exception("A transaction had been started in current scope already.");
            _dbTransaction = _dbConnection.BeginTransaction();
            return _dbTransaction;
        }

        public virtual void RegisterAdd(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            _descriptors.Enqueue(new WorkUnitDescriptor(entity, unitofWorkRepository, WorkUnitType.Addition));
        }

        public virtual void RegisterRemoved(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            _descriptors.Enqueue(new WorkUnitDescriptor(entity, unitofWorkRepository, WorkUnitType.Deletion));
        }

        public virtual void RegisterUpdate(IAggregateRoot entity, IUnitOfWorkRepository unitofWorkRepository)
        {
            _descriptors.Enqueue(new WorkUnitDescriptor(entity, unitofWorkRepository, WorkUnitType.Update));
        }

        private IDbTransaction GetOrStartTransaction()
        {
            if (GetCurrentDbTransaction() != null) return GetCurrentDbTransaction();
            return BeginTransaction(IsolationLevel);
        }

        private async Task PersistAsync(WorkUnitDescriptor descriptor, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            switch (descriptor.WorkUnitType)
            {
                case WorkUnitType.Addition:
                    await descriptor.UnitOfWorkRepository.PersistCreationOfAsync(descriptor.Entity, dbConnection, dbTransaction);
                    break;

                case WorkUnitType.Deletion:
                    await descriptor.UnitOfWorkRepository.PersistDeletionOfAsync(descriptor.Entity, dbConnection, dbTransaction);
                    break;

                case WorkUnitType.Update:
                    await descriptor.UnitOfWorkRepository.PersistUpdateOfAsync(descriptor.Entity, dbConnection, dbTransaction);
                    break;
            }
        }

        public virtual void Dispose()
        {
            if (_dbTransaction != null) _dbTransaction.Dispose();
            if (_dbConnection != null) _dbConnection.Dispose();
        }
    }
}
