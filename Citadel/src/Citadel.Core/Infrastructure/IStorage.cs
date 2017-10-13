using System.Data;
using System.Threading.Tasks;

namespace Citadel.Infrastructure
{
    public interface IStorage
    {
        Task EnsureCreateAsync(IDbConnection dbConnection);
    }
}
