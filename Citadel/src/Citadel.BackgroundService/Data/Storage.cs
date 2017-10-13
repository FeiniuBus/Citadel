using Citadel.Infrastructure;
using Dapper;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.BackgroundService.Data
{
    public class Storage : IStorage
    {
        public async Task EnsureCreateAsync(IDbConnection dbConnection)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"CREATE TABLE IF NOT EXISTS ""public"".""Untitled"" (");
            sb.AppendLine(@"""Id"" varchar(64) NOT NULL,");
            sb.AppendLine(@"""Expression"" varchar(1024) NOT NULL,");
            sb.AppendLine(@"""MethodCall"" text NOT NULL,");
            sb.AppendLine(@"""State"" int2 NOT NULL,");
            sb.AppendLine(@"""StateName"" varchar(32) NOT NULL,");
            sb.AppendLine(@"""CreationTime"" timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,");
            sb.AppendLine(@"PRIMARY KEY(""Id"")");
            sb.AppendLine(@");");
            await dbConnection.ExecuteAsync(sb.ToString());
        }
    }
}
