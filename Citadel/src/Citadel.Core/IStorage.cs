using System.Data;
using System.Threading.Tasks;

namespace Citadel
{
    public interface IStorage
    {
        Task EnsureCreateAsync(IDbConnection dbConnection);
    }
}
