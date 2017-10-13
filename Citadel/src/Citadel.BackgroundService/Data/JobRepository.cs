using Citadel.Abstracts;
using Citadel.BackgroundService.Data.DomainModel;
using Citadel.Infrastructure;
using Dapper;
using System;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.BackgroundService.Data
{
    public class JobRepository : AbstractRepository<Job>
    {
        public JobRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task PersistCreationOfAsync(Job entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            var statement = BuildInsertSql();
            await dbConnection.ExecuteAsync(sql: statement, param: new
            {
                Id = Guid.NewGuid().ToString("N"),
                Expression = entity.Expression,
                MethodCall = entity.MethodCall,
                State = (short)entity.State,
                StateName = entity.StateName
            }, transaction: dbTransaction);
        }

        public override Task PersistDeletionOfAsync(Job entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            return Task.CompletedTask;
        }
        
        public override async Task PersistUpdateOfAsync(Job entity, IDbConnection dbConnection, IDbTransaction dbTransaction)
        {
            var statement = BuildUpdateSql();
            await dbConnection.ExecuteAsync(sql: statement, param: new
            {
                State = (short)entity.State,
                StateName = entity.StateName,
                Id = entity.Id
            }, transaction: dbTransaction);
        }

        private string BuildInsertSql()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"INSERT INTO ""public"".""background.job"" ");
            sb.AppendLine(@"""Id"", ""Expression"", ""MethodCall"", ""State"", ""StateName"")");
            sb.AppendLine(@"VALUES");
            sb.AppendLine(@"(@Id,@Expression,@MethodCall,@State,@StateName)");
            return sb.ToString();
        }

        private string BuildUpdateSql()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"UPDATE ""public"".""background.job"" ");
            sb.AppendLine(@"SET");
            sb.AppendLine(@"""State"" = @State,");
            sb.AppendLine(@"""StateName"" = @StateName,");
            sb.AppendLine(@"WHERE");
            sb.AppendLine(@"""Id"" = @Id;");
            return sb.ToString();
        }
    }
}
