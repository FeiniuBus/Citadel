using System.Data;
using System.Threading.Tasks;

namespace Citadel
{
    public interface IPublisher
    {
        Task PublishAsync<T>(MessageDescriptor descriptor, IDbConnection dbConnection, IDbTransaction dbTransaction);
    }
}
