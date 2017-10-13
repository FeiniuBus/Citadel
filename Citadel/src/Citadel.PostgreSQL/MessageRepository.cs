using Citadel.Abstracts;
using Citadel.Postgre.DomainModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Threading.Tasks;
using Citadel.Infrastructure;
using Dapper;
using System.Linq;

namespace Citadel.Postgre
{
    public class MessageRepository : AbstractRepository<Message>
    {
        public MessageRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public async Task<Message> FindOneAsync(string id)
        {
            var dbConnection = UnitOfWork.GetDbConnection();
            await dbConnection.Open

            Message lookUp = null;
            var query = await dbConnection.QueryAsync<Message, MessageClaim, Message>(BuildFindOneSQL(), (message, claim) =>
            {
                if (lookUp == null || lookUp.Id != message.Id) lookUp = message;

                lookUp.Claims.Add(claim);

                return lookUp;
            });

            return query?.FirstOrDefault();
        }

        public async Task<Message> FindFirstAsync()
        {

        }

        public override async Task PersistCreationOfAsync(Message entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            var insertStatement = BuildInsertMessageSQL();
            entity.Id = Guid.NewGuid().ToString("N");
            await dbConnection.ExecuteAsync(insertStatement, new
            {
                Id = entity.Id,
                MessageType = entity.MessageType,
                Content = entity.Content,
                State = (short)entity.State,
                StateName = entity.StateName
            }, dbTransaction);
            var replaceStatement = BuildReplaceMessageClaimSQL();
            foreach(var claim in entity.Claims)
            {
                await dbConnection.ExecuteAsync(replaceStatement, new
                {
                    Id = Guid.NewGuid().ToString("N"),
                    MessageId = entity.Id,
                    Name = claim.Name,
                    ValueType = claim.ValueType.ToString(),
                    Value = claim.Value,
                }, dbTransaction);
            }
        }

        public override Task PersistDeletionOfAsync(Message entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return Task.CompletedTask;
        }

        public override async Task PersistUpdateOfAsync(Message entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            var updateStatement = BuildUpdateMessageSQL();
            await dbConnection.ExecuteAsync(updateStatement, new { State = (short)entity.State, entity.StateName, entity.Id }, dbTransaction);

            var trackedItems = entity.Claims.GetTrackedItems();
            var needRemove = trackedItems.Where(x => x.Activity == TrackedItemActivity.Remove);
            var needReplace = trackedItems.Where(x => x.Activity == TrackedItemActivity.None || x.Activity == TrackedItemActivity.Add);

            var removeTasks = trackedItems.Where(x => x.Activity == TrackedItemActivity.Remove).Select(x => dbConnection.ExecuteAsync(BuildRemoveMessageClaimSQL(), new { Id = x.Item.Id }, dbTransaction));
            var replaceTasks = trackedItems.Where(x => x.Activity == TrackedItemActivity.None || x.Activity == TrackedItemActivity.Add)
                .Select(x=>dbConnection.ExecuteAsync(BuildReplaceMessageClaimSQL(),
               new
               {
                   Id = Guid.NewGuid().ToString("N"),
                   MessageId = entity.Id,
                   Name = x.Item.Name,
                   ValueType = x.Item.ValueType.ToString(),
                   Value = x.Item.Value,
               }, dbTransaction));

            foreach(var task in removeTasks)
            {
                await task;
            }
            foreach(var task in replaceTasks)
            {
                await task;
            }
        }

        private string BuildInsertMessageSQL()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"INSERT INTO ""citadel.messages"" ( ""Id"", ""MessageType"", ""Content"", ""State"", ""StateName"" )");
            sb.AppendLine(@"VALUES (");
            sb.AppendLine(@"    @Id,");
            sb.AppendLine(@"    @MessageType,");
            sb.AppendLine(@"    @Content,");
            sb.AppendLine(@"    @State,");
            sb.AppendLine(@"    @StateName");
            sb.AppendLine(@" );");
            return sb.ToString();
        }

        private string BuildUpdateMessageSQL()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"UPDATE ""citadel.messages"" ");
            sb.AppendLine(@"SET ");
            sb.AppendLine(@"""State"" = @State, ");
            sb.AppendLine(@"""StateName"" = @StateName ");
            sb.AppendLine(@"WHERE ");
            sb.AppendLine(@"""Id"" = @Id;");
            return sb.ToString();
        }

        private string BuildReplaceMessageClaimSQL()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"INSERT INTO ""citadel.message_claims"" ( ""Id"", ""MessageId"", ""Name"", ""ValueType"", ""Value"" )");
            sb.AppendLine(@"VALUES (");
            sb.AppendLine(@"@Id,");
            sb.AppendLine(@"@MessageId,");
            sb.AppendLine(@"@Name,");
            sb.AppendLine(@"@ValueType,");
            sb.AppendLine(@"@Value");
            sb.AppendLine(@" )");
            sb.AppendLine(@"ON CONFLICT (MessageId, Name) DO UPDATE ");
            sb.AppendLine(@"    SET ""ValueType""=@ValueType,");
            sb.AppendLine(@"           ""Value""=@Value");
            sb.AppendLine(@";");
            return sb.ToString();
        }

        private string BuildRemoveMessageClaimSQL()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"DELETE FROM ""citadel.message_claims"" WHERE ""Id""=@Id");
            return sb.ToString();
        }

        private string BuildFindOneSQL()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"SELECT");
            sb.AppendLine(@"message.""Id"",");
            sb.AppendLine(@"message.""MessageType"",");
            sb.AppendLine(@"message.""Content"",");
            sb.AppendLine(@"message.""State"",");
            sb.AppendLine(@"message.""StateName"",");
            sb.AppendLine(@"message.""CreationTime"",");
            sb.AppendLine(@"claim.""Id"",");
            sb.AppendLine(@"claim.""MessageId"",");
            sb.AppendLine(@"claim.""Name"",");
            sb.AppendLine(@"claim.""ValueType"",");
            sb.AppendLine(@"claim.""Value"",");
            sb.AppendLine(@"claim.""CreationTime""");
            sb.AppendLine(@"FROM");
            sb.AppendLine(@"""citadel.messages"" AS message");
            sb.AppendLine(@"INNER JOIN ""citadel.message_claims"" AS claim ON claim.""MessageId"" = message.""Id""");
            sb.AppendLine(@"WHERE");
            sb.AppendLine(@"message.""Id"" = @Id");
            sb.AppendLine(@"ORDER BY");
            sb.AppendLine(@"message.""Id"",");
            sb.AppendLine(@"message.""CreationTime"" ASC");
            sb.AppendLine(@"LIMIT 1;");
            return sb.ToString();
        }
    }
}
