using Citadel.Data;
using Citadel.Infrastructure;
using Dapper;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.Postgre
{
    public class Storage : IStorage
    {
        private readonly DbConnectionFactoryOptions _dbConnectionFactoryOptions;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public Storage(DbConnectionFactoryOptions dbConnectionFactoryOptions, IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactoryOptions = dbConnectionFactoryOptions;
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task EnsureCreateAsync(IDbConnection dbConnection)
        {
            var sb = new StringBuilder();
            sb.AppendLine($@"CREATE TABLE IF NOT EXISTS ""{_dbConnectionFactoryOptions.Schema}"".""citadel.messages"" (");
            sb.AppendLine(@"  ""Id"" varchar(64)  NOT NULL,");
            sb.AppendLine(@"  ""Exchange"" varchar(255)  NOT NULL,");
            sb.AppendLine(@"  ""Topic"" varchar(255) NOT NULL,");
            sb.AppendLine(@"  ""MessageType"" varchar(16) NOT NULL,");
            sb.AppendLine(@"  ""Content"" text  NOT NULL,");
            sb.AppendLine(@"  ""State"" int2 NOT NULL,");
            sb.AppendLine(@"  ""StateName"" varchar(16) NOT NULL,");
            sb.AppendLine(@"  ""CreationTime"" timestamp(6) NOT NULL DEFAULT now()");
            sb.AppendLine(@")");
            sb.AppendLine(@";");
            sb.AppendLine($@"ALTER TABLE ""{_dbConnectionFactoryOptions.Schema}"".""citadel.messages"" ADD CONSTRAINT ""citadel.messages_pkey"" PRIMARY KEY (""Id"");");

            sb.AppendLine($@"CREATE TABLE  IF NOT EXISTS  ""{_dbConnectionFactoryOptions.Schema}"".""citadel.message_claims"" (");
            sb.AppendLine(@"  ""Id"" varchar(64)  NOT NULL,");
            sb.AppendLine(@"  ""MessageId"" varchar(64) NOT NULL,");
            sb.AppendLine(@"  ""Name"" varchar(255) NOT NULL,");
            sb.AppendLine(@"  ""ValueType"" varchar(512) NOT NULL,");
            sb.AppendLine(@"  ""Value"" text  NOT NULL,");
            sb.AppendLine(@"  ""CreationTime"" timestamp(6) NOT NULL DEFAULT now()");
            sb.AppendLine(@")");
            sb.AppendLine(@"; ");

            sb.AppendLine($@"ALTER TABLE ""{_dbConnectionFactoryOptions.Schema}"".""citadel.message_claims"" ADD CONSTRAINT ""citadel.message_claims_pkey"" PRIMARY KEY (""Id"");");
            sb.AppendLine($@"ALTER TABLE ""{_dbConnectionFactoryOptions.Schema}"".""citadel.message_claims"" ADD CONSTRAINT ""FK_MessageClaims_MessageId_Messages_Id"" FOREIGN KEY (""MessageId"") REFERENCES ""{_dbConnectionFactoryOptions.Schema}"".""citadel.messages"" (""Id"") ON DELETE CASCADE ON UPDATE CASCADE;");

            await dbConnection.ExecuteAsync(sb.ToString());
        }
    }
}
