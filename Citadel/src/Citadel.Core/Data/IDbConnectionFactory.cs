using System.Data;

namespace Citadel.Data
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateDbConnection(string connectionString);
    }
}
