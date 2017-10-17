using Citadel.Data;
using Dapper;
using System.Text;
using System.Threading.Tasks;

namespace Citadel.BackgroundService.Server
{
    public class JobPersistenter
    {
        private readonly IMessagePersistenter _messagePersistenter;
        private readonly IDbConnectionFactory _dbConnectionFactory;
        private readonly DbConnectionFactoryOptions _dbConnectionFactoryOptions;
        private readonly BackgroundServiceOptions _backgroundServiceOptions;

        public JobPersistenter(IMessagePersistenter messagePersistenter, IDbConnectionFactory dbConnectionFactory, DbConnectionFactoryOptions dbConnectionFactoryOptions, BackgroundServiceOptions backgroundServiceOptions)
        {
            _messagePersistenter = messagePersistenter;
            _dbConnectionFactory = dbConnectionFactory;
            _dbConnectionFactoryOptions = dbConnectionFactoryOptions;
            _backgroundServiceOptions = backgroundServiceOptions;
        }

        public async Task<Job> AddJobAsync(JobInfo jobInfo)
        {
            var job = Job.CreateJob(jobInfo, _backgroundServiceOptions);
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
            sb.AppendLine($@"INSERT INTO ""{_dbConnectionFactoryOptions.Schema}"".""background.jobs""(""MessageId"", ""Expression"") VALUES (@MessageId, @Expression);");
            return sb.ToString();
        }
    }
}
