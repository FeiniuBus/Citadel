using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.Infrastructure
{
    public interface IRepository<TAggregateRoot>
        where TAggregateRoot : IAggregateRoot
    {
        Task<int> AddAsync(TAggregateRoot entity);
        Task<int> UpdateAsync(TAggregateRoot entity);
        Task<int> RemoveAsync(TAggregateRoot entity);
    }
}
