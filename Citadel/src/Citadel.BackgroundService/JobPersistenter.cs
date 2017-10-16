using Citadel.BackgroundService.Data.DomainModel;
using Citadel.Data;
using Dapper;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.BackgroundService
{
    public class JobPersistenter
    {
        private readonly IMessagePersistenter _messagePersistenter;
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public JobPersistenter(IMessagePersistenter messagePersistenter, IDbConnectionFactory dbConnectionFactory)
        {
            _messagePersistenter = messagePersistenter;
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<Job> AddJobAsync(JobInfo jobInfo)
        {
            var job = Job.CreateJob(jobInfo);
            var connection = _dbConnectionFactory.CreateDbConnection();
            connection.Open();
            var transaction = connection.BeginTransaction();
            var messageId = await _messagePersistenter.AddMessageAsync(job.Message, connection, transaction);
            await connection.ExecuteAsync(BuildInsertSQL(), new { MessageId = job.Message.Id, Expression = job.Expression });
            transaction.Commit();
            connection.Close();
            connection.Dispose();
            return job;
        }

        private string BuildInsertSQL()
        {
            var sb = new StringBuilder();
            sb.AppendLine(@"INSERT INTO ""background.jobs""(""MessageId"", ""Expression"") VALUES (@MessageId, @Expression);");
            return sb.ToString();
        }
    }
}
