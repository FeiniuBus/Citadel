using Citadel.BackgroundService.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Citadel.BackgroundService
{
    public class JobInfo
    {
        private JobInfo(JobKind jobKind, Expression<Action> methodCall)
        {
            JobId = Guid.NewGuid().ToString("N");
            JobKind = jobKind;
        }

        public string JobId { get; set; }
        public JobKind JobKind { get; set; }
        public Expression<Action> MethodCall { get; set; }
        public IReadOnlyDictionary<string, object> Properties { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
