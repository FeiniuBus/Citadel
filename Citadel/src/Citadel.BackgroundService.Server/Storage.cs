using Citadel.Data;
using Citadel.Infrastructure;
using Dapper;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.BackgroundService.Server
{
    public class Storage : IStorage
    {
        private readonly DbConnectionFactoryOptions _dbConnectionFactoryOptions;

        public Storage(DbConnectionFactoryOptions dbConnectionFactoryOptions)
        {
            _dbConnectionFactoryOptions = dbConnectionFactoryOptions;
        }

        public async Task EnsureCreateAsync(IDbConnection dbConnection)
        {
            var sb = new StringBuilder();
            sb.AppendLine($@"CREATE TABLE IF NOT EXISTS ""{_dbConnectionFactoryOptions.Schema}"".""background.jobs"" (");
            sb.AppendLine(@"   ""MessageId"" varchar(64) NOT NULL,");
            sb.AppendLine(@"   ""Expression"" text NOT NULL");
            sb.AppendLine(@")");
            sb.AppendLine(@"; ");
            sb.AppendLine($@"ALTER TABLE ""{_dbConnectionFactoryOptions.Schema}"".""background.jobs"" ADD CONSTRAINT ""background.jobs_pkey"" PRIMARY KEY (""MessageId"");");
            sb.AppendLine($@"ALTER TABLE ""{_dbConnectionFactoryOptions.Schema}"".""background.jobs"" ADD CONSTRAINT ""FK_BJ_MessageId_Message_Id"" FOREIGN KEY (""MessageId"") REFERENCES ""{_dbConnectionFactoryOptions.Schema}"".""citadel.messages"" (""Id"") ON DELETE CASCADE ON UPDATE CASCADE;");
            await dbConnection.ExecuteAsync(sb.ToString());
        }
    }
}
