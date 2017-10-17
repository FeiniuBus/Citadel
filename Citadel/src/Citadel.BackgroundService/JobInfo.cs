using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Citadel.BackgroundService
{
    public class JobInfo
    {
        private JobInfo(Expression<Action> methodCall, TimeSpan delayTime)
        {
            JobId = Guid.NewGuid().ToString("N");
            MethodCall = methodCall;
            DelayTime = delayTime;
            CreationTime = DateTime.Now;
        }

        public string JobId { get; set; }
        public Expression<Action> MethodCall { get; set; }
        public IReadOnlyDictionary<string, object> Properties { get; set; }
        public TimeSpan DelayTime { get; set; }
        public DateTime CreationTime { get; set; }

        public static JobInfo CreateDelayJob(Expression<Action> methodCall, TimeSpan delayTime)
        {
            return new JobInfo(methodCall, delayTime);
        }
    }
}
