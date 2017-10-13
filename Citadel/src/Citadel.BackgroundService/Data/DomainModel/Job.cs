using Citadel.BackgroundService.Infrastructure;
using Citadel.Infrastructure;
using System;

namespace Citadel.BackgroundService.Data.DomainModel
{
    public class Job : IEntity<string>, IAggregateRoot
    {
        public string Id { get; set; }
        public string Expression { get; set; }
        public string MethodCall { get; set; }
        public JobState State { get; set; }
        public string StateName => State.ToString();
        public DateTime CreationTime { get; set; }
    }
}
