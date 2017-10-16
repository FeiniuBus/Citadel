using Citadel.BackgroundService.Infrastructure;
using Citadel.Infrastructure;
using System;
using System.Reflection;

namespace Citadel.BackgroundService.Data.DomainModel
{
    public class Job 
    {
        public Job()
        {

        }

        public Job(JobInfo jobInfo)
        {
            Id = jobInfo.JobId;
            Expression = jobInfo.MethodCall.ToString();
            MethodCall = ExpressionJsonConvert.Serialize(jobInfo.MethodCall, Assembly.GetExecutingAssembly());
            State = JobState.Scheduled;
            CreationTime = DateTime.Now;
        }

        public string Id { get; set; }
        public string Expression { get; set; }
        public string MethodCall { get; set; }
        public JobState State { get; set; }
        public string StateName => State.ToString();
        public DateTime CreationTime { get; set; }
    }
}
