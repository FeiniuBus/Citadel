using Citadel.Data;
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
        public string Expression { get; set; }

        public Message Message { get; set; }

        public static Job CreateJob(JobInfo jobInfo)
        {
            var job = new Job()
            {
                Message = new Message()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    MessageType = "BackgroundJob",
                    State = MessageState.Scheduled,
                    Content = ExpressionJsonConvert.Serialize(jobInfo.MethodCall, Assembly.GetExecutingAssembly()),
                    CreationTime = DateTime.Now,
                },
                Expression = jobInfo.MethodCall.ToString(),
            };
            return job;
        }
    }
}
