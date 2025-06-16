using System;
using System.Collections.Generic;

namespace External.Product.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public IDictionary<string, string[]> Errors { get; }

        public ValidationException(IDictionary<string, string[]> errors)
        {
            this.Errors = errors;
        }
    }
}
