using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Teronis.Microsoft.JSInterop.Dynamic
{
    [Serializable]
    public class ParameterListException : AggregateException
    {
        public ParameterListException() 
        { }

        public ParameterListException(IEnumerable<Exception> innerExceptions) 
            : base(innerExceptions) { }

        public ParameterListException(params Exception[] innerExceptions) 
            : base(innerExceptions) { }

        public ParameterListException(string? message) 
            : base(message) { }

        public ParameterListException(string? message, IEnumerable<Exception> innerExceptions) 
            : base(message, innerExceptions) { }

        public ParameterListException(string? message, Exception innerException) 
            : base(message, innerException) { }

        public ParameterListException(string? message, params Exception[] innerExceptions) 
            : base(message, innerExceptions) { }

        protected ParameterListException(SerializationInfo info, StreamingContext context) 
            : base(info, context) { }
    }
}
