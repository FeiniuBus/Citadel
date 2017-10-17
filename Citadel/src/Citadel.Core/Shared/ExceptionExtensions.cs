using System;
using System.Collections.Generic;

namespace Citadel.Shared
{
    public static class ExceptionExtensions
    {
        public static IEnumerable<string> GetErrors(this Exception exception)
        {
            Exception e = exception;
            var errors = new List<string>();
            while(e != null)
            {
                errors.Add(e.Message);
                e = e.InnerException;
            }
            return errors;
        }
    }
}
