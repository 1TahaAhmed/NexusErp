using System;
using System.Collections.Generic;
using System.Text;

namespace Nexus.Erp.Domain.ExceptionHandling
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        { }
    }
}
