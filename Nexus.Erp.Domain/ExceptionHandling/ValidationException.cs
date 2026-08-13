using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.ExceptionHandling
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors) : base("One or more validation failures have occurred.")
        {
            Errors = errors.ToDictionary(
            kvp => kvp.Key,
            kvp => string.Join(", ", kvp.Value)
            );
        }
    }
}
