using System.Data;
using System.Threading.Tasks;

namespace Citadel.Shared
{
    public static class IDbConnectionExtensions
    {
        public static async Task OpenAsync(this IDbConnection dbConnection)
        {
            await Task.Run(() => dbConnection.Open());
        }
    }
}
