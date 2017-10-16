using Citadel.Data;
using System.Data;

namespace Citadel.Postgre
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        private readonly DbConnectionFactoryOptions _dbConnectionFactoryOptions;

        public DbConnectionFactory(DbConnectionFactoryOptions dbConnectionFactoryOptions)
        {
            _dbConnectionFactoryOptions = dbConnectionFactoryOptions;
        }

        public IDbConnection CreateDbConnection()
        {
            return new Npgsql.NpgsqlConnection(_dbConnectionFactoryOptions.ConnectionString);
        }
    }
}
