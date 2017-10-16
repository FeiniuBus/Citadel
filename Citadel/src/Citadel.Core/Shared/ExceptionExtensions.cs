using System;
using System.Collections.Generic;

namespace Citadel.Shared
{
    internal static class ExceptionExtensions
    {
        internal static IEnumerable<string> GetErrors(this Exception exception)
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
