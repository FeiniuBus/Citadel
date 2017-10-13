using System.Data;
using System.Threading.Tasks;

namespace Citadel.Shared
{
    internal static class IDbConnectionExtensions
    {
        internal static async Task OpenAsync(this IDbConnection dbConnection)
        {
            await Task.Run(() => dbConnection.Open());
        }
    }
}
