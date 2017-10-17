using Citadel.Data;
using Citadel.Infrastructure;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Citadel.BackgroundService.Server
{
    public class Job 
    {
        public Job()
        {

        }
        public string Expression { get; set; }

        public Message Message { get; set; }

        public static Job CreateJob(JobInfo jobInfo, BackgroundServiceOptions backgroundServiceOptions)
        {
            var job = new Job()
            {
                Message = new Message()
                {
                    Id = Guid.NewGuid().ToString("N"),
                    MessageType = "BackgroundJob",
                    Exchange = backgroundServiceOptions.Exchange,
                    Topic = backgroundServiceOptions.Topic,
                    State = MessageState.Scheduled,
                    Content = ExpressionJsonConvert.Serialize(jobInfo.MethodCall, Assembly.GetExecutingAssembly()),
                    CreationTime = DateTime.Now,
                },
                Expression = jobInfo.MethodCall.ToString(),
            };
            job.Message.Claims.Add(new MessageClaim
            {
                Id = Guid.NewGuid().ToString("N"),
                CreationTime = DateTime.Now,
                MessageId = job.Message.Id,
                Name = "Headers",
                Value = JsonConvert.SerializeObject(new Dictionary<string, object>()
                {
                    ["x-delay"] = (int)jobInfo.DelayTime.TotalMilliseconds
                }),
                ValueType = ClaimValueType.Create(typeof(Dictionary<string, object>))
            });
            return job;
        }
    }
}
