using Citadel.Infrastructure;
using Dapper;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.Postgre
{
    public class Storage : IStorage
    {
        public async Task EnsureCreateAsync(IDbConnection dbConnection)
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"CREATE TABLE IF NOT EXISTS ""citadel.messages"" (");
            sb.AppendLine(@"""Id"" varchar(64) NOT NULL,");
            sb.AppendLine(@"""MessageType"" varchar(16) NOT NULL,");
            sb.AppendLine(@"""Content"" text  NOT NULL,");
            sb.AppendLine(@"""State"" int2 NOT NULL,");
            sb.AppendLine(@"""StateName"" varchar(16) NOT NULL,");
            sb.AppendLine(@"""CreationTime"" timestamp(6) NOT NULL DEFAULT now(),");
            sb.AppendLine(@"PRIMARY KEY(""Id"")");
            sb.AppendLine(@"); ");

            sb.AppendLine(@"CREATE TABLE  IF NOT EXISTS ""citadel.message_claims"" (");
            sb.AppendLine(@"""Id"" varchar(64) NOT NULL,");
            sb.AppendLine(@"""MessageId"" varchar(64) NOT NULL,");
            sb.AppendLine(@"""Name"" varchar(255) NOT NULL,");
            sb.AppendLine(@"""ValueType"" varchar(512) NOT NULL,");
            sb.AppendLine(@"""Value"" text NOT NULL,");
            sb.AppendLine(@"""CreationTime"" timestamp NOT NULL DEFAULT now(),");
            sb.AppendLine(@"PRIMARY KEY(""Id"")");
            sb.AppendLine(@");");

            sb.AppendLine(@"ALTER TABLE ""citadel.message_claims"" ");
            sb.AppendLine(@"        ADD CONSTRAINT ""FK_MessageClaims_MessageId_Messages_Id"" FOREIGN KEY(""MessageId"") REFERENCES ""citadel.messages"" (""Id"") ON DELETE CASCADE ON UPDATE CASCADE;");

            await dbConnection.ExecuteAsync(sb.ToString());
        }
    }
}
