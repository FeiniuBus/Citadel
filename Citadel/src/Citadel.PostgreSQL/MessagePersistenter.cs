using Citadel.Data;
using Citadel.Infrastructure;
using Dapper;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.Postgre
{
    public class MessagePersistenter : IMessagePersistenter
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly DbConnectionFactoryOptions _dbConnectionFactoryOptions;

        public MessagePersistenter(IDbConnectionFactory dbConnectionFactory, DbConnectionFactoryOptions dbConnectionFactoryOptions)
        {
            _dbConnectionFactory = dbConnectionFactory;
            _dbConnectionFactoryOptions = dbConnectionFactoryOptions;
        }

        public async Task<string> AddMessageAsync(Message message, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            await dbConnection.ExecuteAsync(BuildInsertMessageSQL(), new
            {
                message.Id,
                message.Exchange,
                message.Topic,
                message.MessageType,
                message.Content,
                message.State,
                message.StateName,
                message.CreationTime
            }, dbTransaction);

            if(message.Claims?.Any() == true)
            {
                foreach(var claim in message.Claims)
                {
                    await dbConnection.ExecuteAsync(BuildReplaceMessageClaimSQL(), new
                    {
                        Id = claim.Id,
                        MessageId = claim.MessageId,
                        Name = claim.Name,
                        ValueType = claim.ValueType.ToString(),
                        Value = claim.Value
                    }, dbTransaction);
                }
            }
            return message.Id;
        }

        public async Task ChangeStateAsync(string messageId, MessageState messageState, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            await dbConnection.ExecuteAsync(BuildChangeStateSQL(), new
            {
                State = (short)messageState,
                StateName = messageState.ToString(),
                Id = messageId
            }, dbTransaction);
        }

        private string BuildInsertMessageSQL()
        {
            var sb = new StringBuilder();
            sb.AppendLine($@"INSERT INTO ""{_dbConnectionFactoryOptions.Schema}"".""citadel.messages"" (");
            sb.AppendLine(@"    ""Id"",");
            sb.AppendLine(@"    ""Exchange"",");
            sb.AppendLine(@"    ""Topic"",");
            sb.AppendLine(@"    ""MessageType"",");
            sb.AppendLine(@"    ""Content"",");
            sb.AppendLine(@"    ""State"",");
            sb.AppendLine(@"    ""StateName"",");
            sb.AppendLine(@"    ""CreationTime""");
            sb.AppendLine(@") VALUES(");
            sb.AppendLine(@"    @Id,");
            sb.AppendLine(@"    @Exchange,");
            sb.AppendLine(@"    @Topic,");
            sb.AppendLine(@"    @MessageType,");
            sb.AppendLine(@"    @Content,");
            sb.AppendLine(@"    @State,");
            sb.AppendLine(@"    @StateName,");
            sb.AppendLine(@"    @CreationTime");
            sb.AppendLine(@");");
            return sb.ToString();
        }

        private string BuildReplaceMessageClaimSQL()
        {
            var sb = new StringBuilder();
            sb.AppendLine($@"INSERT INTO ""{_dbConnectionFactoryOptions.Schema}"".""citadel.message_claims"" ( ""Id"", ""MessageId"", ""Name"", ""ValueType"", ""Value"" )");
            sb.AppendLine(@"VALUES (");
            sb.AppendLine(@"@Id,");
            sb.AppendLine(@"@MessageId,");
            sb.AppendLine(@"@Name,");
            sb.AppendLine(@"@ValueType,");
            sb.AppendLine(@"@Value");
            sb.AppendLine(@" )");
            sb.AppendLine(@";");
            return sb.ToString();
        }

        private string BuildChangeStateSQL()
        {
            var sb = new StringBuilder();
            sb.AppendLine($@"UPDATE ""{_dbConnectionFactoryOptions.Schema}"".""citadel.messages"" ");
            sb.AppendLine(@"SET ");
            sb.AppendLine(@"""State"" = @State, ");
            sb.AppendLine(@"""StateName"" = @StateName ");
            sb.AppendLine(@"WHERE ");
            sb.AppendLine(@"""Id"" = @Id;");
            return sb.ToString();
        }
    }
}
