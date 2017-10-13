using Citadel.Infrastructure;
using System.Threading.Tasks;

namespace Citadel
{
    public interface IService<TStorage, TUnitOfWork>
        where TStorage : IStorage
        where TUnitOfWork : IUnitOfWork
    {
        Task PublishAsync(MessageDescriptor messageDescriptor);
    }
}
