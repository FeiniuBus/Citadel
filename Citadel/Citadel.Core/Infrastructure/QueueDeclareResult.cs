using System;
using System.Collections.Generic;

namespace Citadel.Infrastructure
{
    public class QueueDeclareResult
    {
        private readonly IReadOnlyDictionary<string, object> _data;

        private QueueDeclareResult(IEnumerable<string> errors)
        {
            Succeeded = false;
            Errors = errors;
        }

        private QueueDeclareResult(IQueue queue)
        {
            Succeeded = true;
            Queue = queue;
        }

        public bool Succeeded { get; private set; }

        public IEnumerable<string> Errors { get; private set; }

        public IQueue Queue { get; private set; }

        public static QueueDeclareResult Success(IQueue queue)
            => new QueueDeclareResult(queue);

        public static QueueDeclareResult Failed(IEnumerable<string> errors)
            => new QueueDeclareResult(errors);

        public static QueueDeclareResult Failed(Exception e)
        {
            var lst = new List<string>();
            Exception current = e;
            while(current != null)
            {
                lst.Add(current.Message);
                current = current.InnerException;
            }
            return Failed(lst);
        }
    }
}
