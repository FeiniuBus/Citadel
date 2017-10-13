using Citadel.Infrastructure;
using System;

namespace Citadel.Abstracts
{
    public class WorkUnitDescriptor
    {
        public WorkUnitDescriptor(IAggregateRoot entity, IUnitOfWorkRepository unitOfWorkRepository, WorkUnitType workUnitType)
        {
            Entity = entity;
            UnitOfWorkRepository = unitOfWorkRepository;
            WorkUnitType = workUnitType;
            RegisteredTime = DateTime.Now;
        }

        public IAggregateRoot Entity { get; }
        public IUnitOfWorkRepository UnitOfWorkRepository { get; }
        public WorkUnitType WorkUnitType { get; }
        public DateTime RegisteredTime { get; }
    }
}
